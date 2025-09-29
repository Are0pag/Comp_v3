using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Comp.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
        try {
            var canConnect = Database.CanConnect();
            var isCreated = Database.EnsureCreated();
        }
        catch (Exception ex) {
            Console.WriteLine($"Error during database initialization: {ex.Message}");
        }
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
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    #endif
    }
}