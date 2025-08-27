using Comp_v3.NomDict.View;
using Microsoft.Extensions.DependencyInjection;
using NomDictWindow = Comp_v3.Front.NomDict.View.NomDictWindow;

namespace Comp_v3.Back.Bootstrap.ServiceCollectionExtensions;

public static class NomDictExtension
{
    public static void Register(this IServiceCollection services) {
        services.AddSingleton<NomDictWindowVm>();
        services.AddSingleton<NomDictWindow>();
    }
}