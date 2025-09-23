using Comp.Db;
using Microsoft.EntityFrameworkCore;
using WPF.Services;

namespace Comp_v4;

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
    }
}