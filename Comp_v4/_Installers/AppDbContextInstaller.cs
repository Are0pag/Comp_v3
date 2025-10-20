using Comp.Db;
using DI;
using Microsoft.EntityFrameworkCore;

namespace Comp_v4.Installers;

public class AppDbContextInstaller : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<AppDbContext>()
                 .AsSingleton()
                 .UsingFactoryMethod(() => {
                      var options = new DbContextOptionsBuilder<AppDbContext>()
                                   .UseSqlite(DbConfig.ConnectionString)
                                   .Options;
                      
                      return new AppDbContext(options);
                  });
        
        container.Add<DatabaseInitializer>().AsTransient();
    }
}