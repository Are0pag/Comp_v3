using System.IO;
using CL_Comp_ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;

namespace CL_CompDb;

public class AppDbContext : DbContext
{
    protected readonly string _dbName;
    protected readonly string _folderPath;
    protected readonly string _connectionString;

    public AppDbContext() {
        _dbName = "comp.db";
        _folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        _connectionString = $"data source={Path.Combine(_folderPath, _dbName)}";
    }
    
    public virtual DbSet<ConditionalDesignation> ConditionalDesignations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ConditionalDesignation>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(10);
            entity.Property(e => e.Designation).IsRequired().HasMaxLength(10);
        });
    }
}