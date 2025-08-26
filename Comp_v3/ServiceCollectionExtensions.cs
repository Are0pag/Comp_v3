using Comp_v3.NomDict.View;
using Microsoft.Extensions.DependencyInjection;

namespace Utils.DiExtentions;

public static class ServiceCollectionExtensions
{
    public static void AddNomDictEntities(this IServiceCollection services) {
        services.AddSingleton<NomDictWindowVm>();
        services.AddSingleton<NomDictWindow>();
    }
}