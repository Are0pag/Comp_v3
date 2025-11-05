using Comp_v4._Installers;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.Installers;

public static class DbRegExtension
{
    public static void RegisterDb(this IServiceCollection services) {
        services.AddDbContext<AppDbContext>(options => {
            options.UseSqlite(DbConfig.ConnectionString)
            .EnableDetailedErrors();
        });
        
        services.AddHostedService<DatabaseInitializer>();
        
        services.AddTransient<IRepository<Counterparty>, CounterpartyRepository>();
        services.AddTransient<IRepository<SupplierOrder>, SupplierOrderRepository>();
    }
}