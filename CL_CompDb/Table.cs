using Microsoft.EntityFrameworkCore;
using System.IO;
using CL_Comp_ModelData.TechnicalItems;

namespace CL_CompDb;

public class Table { }

public class AppContext : DbContext
{
    protected readonly string _dbName = "comp.db";
    protected readonly string _folder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    
    public virtual DbSet<TypeSize> TypeSizes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);
        var connectionString = string.Format("data source={0}", Path.Combine(_folder, _dbName));
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<TypeSize>();
        /*modelBuilder.Entity<TypeSize>().HasData(
            new TypeSize() {
                Designation = "Hz",
                OutputsNumber = 2,
                IsUsingSmd = true,
                Description = "Small"
            }
        );*/
    }
}

