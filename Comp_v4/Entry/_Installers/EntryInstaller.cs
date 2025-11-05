using Comp_v4.Entry.Actions;
using Comp_v4.Entry.Entities;
using Comp_v4.Entry.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.Entry._Installers;

public static class EntryInstaller
{
    public static void InstallEntry(this IServiceCollection services) {
        services.AddSingleton<ToolsPanelStateIdle>();
        
        services.AddSingleton<ToolsPanel>(provider => {
            var initialState = provider.GetRequiredService<ToolsPanelStateIdle>();
            var states = new List<BaseToolsPanelState>() {
                initialState,
            };
            return new ToolsPanel(states, initialState);
        });

        services.AddSingleton<OpenNomDictAction>();
        services.AddSingleton<NomDictButVm>();
        
        services.AddSingleton<OpenSupplierOrdersAction>();
        services.AddSingleton<OrdersButVm>();

        services.AddTransient<EntryWindow>();
    }
}