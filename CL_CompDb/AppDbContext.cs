using CL_Comp_ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace CL_CompDb;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) {
    }
    public virtual DbSet<ConditionalDesignation> ConditionalDesignations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ConditionalDesignation>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(ConditionalDesignation.MAX_NAME_LENGTH);
            entity.Property(e => e.Designation).IsRequired().HasMaxLength(ConditionalDesignation.MAX_DESIGNATION_LENGTH);
        });
    }
}