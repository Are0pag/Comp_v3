using Comp_v4._Installers;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Comp_v4.Installers;

public static class DbRegExtension
{
    public static void RegisterDb(this IServiceCollection services) {
        services.AddDbContext<AppDbContext>(options => {
            options.UseSqlite(DbConfig.ConnectionString)
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging();
        });
        
        services.AddLogging(builder => {
            builder.ClearProviders(); // Удаление провайдеров логирования по умолчанию
        });
        
        services.AddHostedService<DatabaseInitializer>();
        
        services.AddTransient<IRepository<Counterparty>, CounterpartyRepository>();
        services.AddTransient<IRepository<SupplierOrder>, SupplierOrderRepository>();
        
        services.AddTransient<IRepository<ConditionalDesignation>, DbRepository<ConditionalDesignation>>();
        services.AddTransient<IRepository<Manufacturer>, DbRepository<Manufacturer>>();
        services.AddTransient<IRepository<MeasurementUnit>, DbRepository<MeasurementUnit>>();
        services.AddTransient<IRepository<TypeSize>, TsRepo>();
        
        services.AddTransient<IRepository<Category>, RepositoryCategory>();
        services.AddTransient<IRepository<GenericParametersSet>, DbRepository<GenericParametersSet>>();

        services.AddTransient<IRepository<Analog>, RepoAnalogs>();
        
        services.AddTransient<IRepository<Component>, RepositoryComponent>();
    }
}