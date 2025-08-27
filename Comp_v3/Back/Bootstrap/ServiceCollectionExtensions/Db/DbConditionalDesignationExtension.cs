using Comp_v3.Back.Config;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v3.Back.Bootstrap.ServiceCollectionExtensions.Db;

public static class DbConditionalDesignationExtension
{
    public static void RegisterConditionalDesignationsTable(this IServiceCollection services) {
        services.AddDbContext<AppDbContext>(options => {
            options.UseSqlite(DbConfig.ConnectionString);
        });
                            
        services.AddTransient<IRepository<ConditionalDesignation>, ConditionalDesignationRepository>();
        services.AddTransient<IConditionalDesignationRepository, ConditionalDesignationRepository>();
    }
}