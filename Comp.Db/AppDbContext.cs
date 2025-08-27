using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) {
    }
    public virtual DbSet<ConditionalDesignation> ConditionalDesignations { get; set; }
    /*public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
    public DbSet<TypeSize> TypeSizes { get; set; }*/
    /* And all of thr tables  */
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}