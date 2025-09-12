using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Comp_v4;

static public class ExceptionLogger
{
    static private readonly ConcurrentQueue<ExceptionLogEntry> _logQueue = new();
    static private readonly string _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
    static private readonly object _fileLock = new();
    static private bool _isInitialized = false;

    static ExceptionLogger() {
        Initialize();
    }

    static public void Log(this Exception exception, object source, bool isThrow = true,
                           [System.Runtime.CompilerServices.CallerMemberName]
                           string memberName = "",
                           [System.Runtime.CompilerServices.CallerFilePath]
                           string filePath = "",
                           [System.Runtime.CompilerServices.CallerLineNumber]
                           int lineNumber = 0) {
        var logEntry = new ExceptionLogEntry {
            Exception = exception,
            Source = source,
            Timestamp = DateTime.Now,
            MemberName = memberName,
            FilePath = filePath,
            LineNumber = lineNumber,
            ShouldThrow = isThrow
        };

        _logQueue.Enqueue(logEntry);

        // Также выводим в консоль для немедленного отображения
        WriteToConsole(logEntry);

        if (isThrow)
            throw exception;
    }

    static private void Initialize() {
        if (_isInitialized) return;

        try {
            if (!Directory.Exists(_logDirectory)) Directory.CreateDirectory(_logDirectory);

            // Запускаем фоновую задачу для обработки логов
            Task.Run(async () => await ProcessLogQueueAsync());

            _isInitialized = true;
            LogInternal("Logger initialized successfully", "ExceptionLogger", LogLevel.Info);
        }
        catch (Exception ex) {
            Debug.WriteLine($"Failed to initialize logger: {ex.Message}");
        }
    }

    static private async Task ProcessLogQueueAsync() {
        while (true) {
            if (_logQueue.TryDequeue(out var logEntry)) await WriteToFileAsync(logEntry);
            await Task.Delay(100);
        }
    }

    static private void WriteToConsole(ExceptionLogEntry logEntry) {
        var consoleColor = GetConsoleColor(logEntry.Exception);
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = consoleColor;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════════");
        Console.WriteLine($"║ EXCEPTION: {logEntry.Exception.GetType().Name}");
        Console.WriteLine($"║ TIME:      {logEntry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}");
        Console.WriteLine($"║ SOURCE:    {logEntry.Source?.GetType().Name ?? "Unknown"}");
        Console.WriteLine($"║ METHOD:    {logEntry.MemberName}");
        Console.WriteLine($"║ LOCATION:  {Path.GetFileName(logEntry.FilePath)}:{logEntry.LineNumber}");
        Console.WriteLine($"║ ASSEMBLY:  {logEntry.Source?.GetType().Assembly.GetName().Name}");
        Console.WriteLine($"║ NAMESPACE: {logEntry.Source?.GetType().Namespace}");
        Console.WriteLine("╟───────────────────────────────────────────────────────────────");
        Console.WriteLine($"║ MESSAGE:   {logEntry.Exception.Message}");
        Console.WriteLine("╟───────────────────────────────────────────────────────────────");
        Console.WriteLine($"║ STACK TRACE:");
        foreach (var line in logEntry.Exception.StackTrace?.Split('\n') ?? Array.Empty<string>()) Console.WriteLine($"║   {line.Trim()}");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════════");
        Console.ForegroundColor = originalColor;
    }

    static private async Task WriteToFileAsync(ExceptionLogEntry logEntry) {
        var logFileName = $"exceptions_{DateTime.Now:yyyyMMdd}.log";
        var logFilePath = Path.Combine(_logDirectory, logFileName);

        var logBuilder = new StringBuilder();
        logBuilder.AppendLine($"[{logEntry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}]");
        logBuilder.AppendLine($"Exception: {logEntry.Exception.GetType().Name}");
        logBuilder.AppendLine($"Source:    {logEntry.Source?.GetType().FullName}");
        logBuilder.AppendLine($"Method:    {logEntry.MemberName}");
        logBuilder.AppendLine($"Location:  {logEntry.FilePath}:{logEntry.LineNumber}");
        logBuilder.AppendLine($"Assembly:  {logEntry.Source?.GetType().Assembly.GetName().Name}");
        logBuilder.AppendLine($"Namespace: {logEntry.Source?.GetType().Namespace}");
        logBuilder.AppendLine($"Message:   {logEntry.Exception.Message}");
        logBuilder.AppendLine("Stack Trace:");
        logBuilder.AppendLine(logEntry.Exception.StackTrace);
        logBuilder.AppendLine(new string('═', 80));
        logBuilder.AppendLine();

        try {
            lock (_fileLock) {
                File.AppendAllText(logFilePath, logBuilder.ToString(), Encoding.UTF8);
            }
        }
        catch (Exception ex) {
            Debug.WriteLine($"Failed to write to log file: {ex.Message}");
        }

        await Task.CompletedTask;
    }

    static private ConsoleColor GetConsoleColor(Exception exception) {
        return exception switch {
            ArgumentException _         => ConsoleColor.Yellow,
            NullReferenceException _    => ConsoleColor.Red,
            InvalidOperationException _ => ConsoleColor.Magenta,
            TimeoutException _          => ConsoleColor.Cyan,
            NotImplementedException _   => ConsoleColor.Blue,
            _                           => ConsoleColor.DarkRed
        };
    }

    static public void LogInfo(string message, object source = null) {
        LogInternal(message, source, LogLevel.Info);
    }

    static public void LogWarning(string message, object source = null) {
        LogInternal(message, source, LogLevel.Warning);
    }

    static public void LogError(string message, object source = null) {
        LogInternal(message, source, LogLevel.Error);
    }

    static private void LogInternal(string message, object source, LogLevel level) {
        var logEntry = new ExceptionLogEntry {
            Exception = new Exception(message),
            Source = source,
            Timestamp = DateTime.Now,
            Level = level
        };

        _logQueue.Enqueue(logEntry);
        WriteToConsole(logEntry);
    }

    static public string GetLogDirectory() {
        return _logDirectory;
    }

    static public void Flush() {
        while (_logQueue.TryDequeue(out var logEntry)) WriteToFileAsync(logEntry).Wait();
    }
}

public class ExceptionLogEntry
{
    public Exception Exception { get; set; }
    public object Source { get; set; }
    public DateTime Timestamp { get; set; }
    public string MemberName { get; set; }
    public string FilePath { get; set; }
    public int LineNumber { get; set; }
    public bool ShouldThrow { get; set; }
    public LogLevel Level { get; set; } = LogLevel.Error;
}

public enum LogLevel
{
    Info,
    Warning,
    Error
}

static public class LoggerExtensions
{
    static public void LogIfNull(this object obj, string parameterName, object source = null) {
        if (obj == null) new ArgumentNullException(parameterName).Log(source);
    }

    static public void LogIfFalse(this bool condition, string message, object source = null) {
        if (!condition) new InvalidOperationException(message).Log(source);
    }

    static public void LogWithTimeout(this Task task, TimeSpan timeout, object source = null) {
        if (!task.Wait(timeout)) new TimeoutException($"Operation timed out after {timeout.TotalSeconds} seconds").Log(source);
    }
}