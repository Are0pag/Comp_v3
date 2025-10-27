/*using Comp_v4._Installers;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
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
        
        container.Add<IRepository<Counterparty>>()
                 .To<CounterpartyRepository>()
                 .AsTransient();
        
        container.Add<IRepository<SupplierOrder>>()
                 .To<SupplierOrderRepository>()
                 .AsTransient();
    }
}*/