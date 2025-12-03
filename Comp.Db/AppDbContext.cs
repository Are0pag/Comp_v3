using Comp.ModelData;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace Comp.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
    }
    
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<ConditionalDesignation> ConditionalDesignations { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
    public DbSet<TypeSize> TypeSizes { get; set; }
    public DbSet<GenericParametersSet> GenericParametersSets { get; set; }
    
    public DbSet<Component> Components { get; set; }
    
    
    public DbSet<Counterparty> Counterparties { get; set; }
    public DbSet<SupplierOrder> SupplierOrders { get; set; }
    public DbSet<OrderPosition> OrderPositions { get; set; }
    public DbSet<PaymentOrder> PaymentOrders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        return await base.SaveChangesAsync(cancellationToken);
    }
}