using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Comp.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
        Console.WriteLine("AppDbContext created");
    #if DEBUG
        Console.WriteLine($"AppDbContext created. Database: {Database.GetDbConnection().Database}");

        try {
            // Проверяем соединение
            var canConnect = Database.CanConnect();
            Console.WriteLine($"Database can connect: {canConnect}");

            // Пытаемся создать базу
            var isCreated = Database.EnsureCreated();
            Console.WriteLine($"Database ensured created: {isCreated}");

            // Проверяем существование таблиц
            var connection = Database.GetDbConnection();
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";
            using var reader = command.ExecuteReader();
            Console.WriteLine("Tables in database:");
            while (reader.Read()) Console.WriteLine($" - {reader.GetString(0)}");
            connection.Close();
        }
        catch (Exception ex) {
            Console.WriteLine($"Error during database initialization: {ex.Message}");
        }
    #endif
    }
    public DbSet<ConditionalDesignation> ConditionalDesignations { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}