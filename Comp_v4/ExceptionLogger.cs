using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Comp_v4
{
    public static class ExceptionLogger
    {
        private static readonly ConcurrentQueue<ExceptionLogEntry> _logQueue = new();
        private static readonly string _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly object _fileLock = new();
        private static bool _isInitialized = false;

        static ExceptionLogger() {
            Initialize();
        }

        public static void Log(this Exception exception, object source, bool isThrow = true, 
                               [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
                               [System.Runtime.CompilerServices.CallerFilePath] string filePath = "",
                               [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
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

        private static void Initialize() {
            if (_isInitialized) return;
            try {
                if (!Directory.Exists(_logDirectory)) {
                    Directory.CreateDirectory(_logDirectory);
                }

                // Запускаем фоновую задачу для обработки логов
                Task.Run(async () => await ProcessLogQueueAsync());

                _isInitialized = true;
                LogInternal("Logger initialized successfully", "ExceptionLogger", LogLevel.Info);
            }
            catch (Exception ex) {
                Debug.WriteLine($"Failed to initialize logger: {ex.Message}");
            }
        }

        private static async Task ProcessLogQueueAsync() {
            while (true) {
                if (_logQueue.TryDequeue(out var logEntry)) {
                    await WriteToFileAsync(logEntry);
                }
                await Task.Delay(100);
            }
        }

        private static void WriteToConsole(ExceptionLogEntry logEntry) {
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
            foreach (var line in logEntry.Exception.StackTrace?.Split('\n') ?? Array.Empty<string>())
            {
                Console.WriteLine($"║   {line.Trim()}");
            }
            Console.WriteLine("╚═══════════════════════════════════════════════════════════════");
            Console.ForegroundColor = originalColor;
        }

        private static async Task WriteToFileAsync(ExceptionLogEntry logEntry)
        {
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

            try
            {
                lock (_fileLock)
                {
                    File.AppendAllText(logFilePath, logBuilder.ToString(), Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to write to log file: {ex.Message}");
            }

            await Task.CompletedTask;
        }

        private static ConsoleColor GetConsoleColor(Exception exception) {
            return exception switch {
                ArgumentException _ => ConsoleColor.Yellow,
                NullReferenceException _ => ConsoleColor.Red,
                InvalidOperationException _ => ConsoleColor.Magenta,
                TimeoutException _ => ConsoleColor.Cyan,
                NotImplementedException _ => ConsoleColor.Blue,
                _ => ConsoleColor.DarkRed
            };
        }

        public static void LogInfo(string message, object source = null)
        {
            LogInternal(message, source, LogLevel.Info);
        }

        public static void LogWarning(string message, object source = null)
        {
            LogInternal(message, source, LogLevel.Warning);
        }

        public static void LogError(string message, object source = null)
        {
            LogInternal(message, source, LogLevel.Error);
        }

        private static void LogInternal(string message, object source, LogLevel level)
        {
            var logEntry = new ExceptionLogEntry
            {
                Exception = new Exception(message),
                Source = source,
                Timestamp = DateTime.Now,
                Level = level
            };

            _logQueue.Enqueue(logEntry);
            WriteToConsole(logEntry);
        }

        public static string GetLogDirectory() => _logDirectory;

        public static void Flush()
        {
            while (_logQueue.TryDequeue(out var logEntry))
            {
                WriteToFileAsync(logEntry).Wait();
            }
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

    public static class LoggerExtensions
    {
        public static void LogIfNull(this object obj, string parameterName, object source = null)
        {
            if (obj == null)
            {
                new ArgumentNullException(parameterName).Log(source);
            }
        }

        public static void LogIfFalse(this bool condition, string message, object source = null)
        {
            if (!condition)
            {
                new InvalidOperationException(message).Log(source);
            }
        }

        public static void LogWithTimeout(this Task task, TimeSpan timeout, object source = null)
        {
            if (!task.Wait(timeout))
            {
                new TimeoutException($"Operation timed out after {timeout.TotalSeconds} seconds").Log(source);
            }
        }
    }
}