using System.Diagnostics;
using System.IO;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Comp.Db;

public class AppDbContext : DbContext
{
    private readonly string _logFilePath;
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        try {
            var canConnect = Database.CanConnect();
            var isCreated = Database.EnsureCreated();
        }
        catch (Exception ex) {
            Console.WriteLine($"Error during database initialization: {ex.Message}");
        }

        var localApplicationData = Environment.SpecialFolder.LocalApplicationData;
        _logFilePath = Path.Combine(Environment.GetFolderPath(localApplicationData),
                                    "Component", 
                                    "database_log.txt");
        
        // Создаем директорию для логов если не существует
        Directory.CreateDirectory(Path.GetDirectoryName(_logFilePath));
    }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<ConditionalDesignation> ConditionalDesignations { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
    public DbSet<TypeSize> TypeSizes { get; set; }
    
    public DbSet<Component> Components { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
    #if DEBUG
        if (!optionsBuilder.IsConfigured)
            optionsBuilder
               .UseSqlite("Your Connection String")
               .EnableSensitiveDataLogging() // Важно: показывает значения параметров
               .EnableDetailedErrors()       // Детальные ошибки
               .LogTo(LogToFile,
                      new[] { DbLoggerCategory.Database.Command.Name },
                      LogLevel.Information,
                      DbContextLoggerOptions.SingleLine | DbContextLoggerOptions.UtcTime);
    #endif
    }

    private void LogToFile(string message) {
        try {
            var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}";
            File.AppendAllText(_logFilePath, logMessage);
            Debug.WriteLine(logMessage); // Также в Output window
        }
        catch (Exception ex) {
            Debug.WriteLine($"Logging failed: {ex.Message}");
        }
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        await LogChangesAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges() {
        LogChangesAsync().GetAwaiter().GetResult();
        return base.SaveChanges();
    }

    private async Task LogChangesAsync() {
        try {
            var changes = GetChangesSummary();
            if (!string.IsNullOrEmpty(changes)) {
                var logMessage =
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - CHANGES TO SAVE:{Environment.NewLine}{changes}{Environment.NewLine}";
                await File.AppendAllTextAsync(_logFilePath, logMessage);
                Debug.WriteLine(logMessage);
            }
        }
        catch (Exception ex) {
            Debug.WriteLine($"Change logging failed: {ex.Message}");
        }
    }

    private string GetChangesSummary() {
        var changes = new List<string>();

        foreach (var entry in ChangeTracker.Entries())
            if (entry.State == EntityState.Added ||
                entry.State == EntityState.Modified ||
                entry.State == EntityState.Deleted) {
                changes.Add($"TABLE: {entry.Metadata.GetTableName()}");
                changes.Add($"STATE: {entry.State}");
                changes.Add($"ENTITY: {entry.Entity.GetType().Name}");

                foreach (var property in entry.Properties) {
                    var originalValue = property.OriginalValue?.ToString() ?? "null";
                    var currentValue = property.CurrentValue?.ToString() ?? "null";

                    if (entry.State == EntityState.Added)
                        changes.Add($"  {property.Metadata.Name}: {currentValue}");
                    else if (entry.State == EntityState.Modified && property.IsModified)
                        changes.Add($"  {property.Metadata.Name}: {originalValue} -> {currentValue}");
                    else if (entry.State == EntityState.Deleted)
                        changes.Add($"  {property.Metadata.Name}: {originalValue}");
                }

                changes.Add("---");
            }

        return changes.Count > 0 ? string.Join(Environment.NewLine, changes) : string.Empty;
    }
}