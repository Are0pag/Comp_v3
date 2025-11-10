using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Operations.Actions.Components;
using Comp_v4.NomDict.View;
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp_v4.NomDict.Vm.Buttons.Components;
using Microsoft.Extensions.DependencyInjection;
using WPF.UCL;

namespace Comp_v4.NomDict.Installers;

public static class NomDictInstallerExt
{
    public static void RegisterNomDict(this IServiceCollection services) {
        services.AddTransient<OneValueWindow>();
        
        RegisterCategories(services);
        Table(services);
    }

    private static void Table(IServiceCollection services) {
        services.AddSingleton<DataGridVm>();
        
        services.AddSingleton<AddCompButtonVm>();
        services.AddSingleton<AddComponentAction>();
        
        States();

        services.AddTransient<NomDictWindow>();

        void States() {
            services.AddSingleton<EditGridState>();
            services.AddSingleton<SelectionGridState>();

            services.AddSingleton<BaseSGridState, SelectionGridState>();
            services.AddSingleton<BaseSGridState, EditGridState>();

            services.AddSingleton<Grid>();
            /*services.AddSingleton<Comp_v4.NomDict.Entities.Grid>(provider => {
                var initState = provider.GetRequiredService<EditGridState>();
                var states = new List<BaseSGridState>() {
                    initState,
                    provider.GetRequiredService<SelectionGridState>()
                };
                return new Comp_v4.NomDict.Entities.Grid(states, initState);
            });*/
        }
    }

    private static void RegisterCategories(IServiceCollection services) {
        services.AddSingleton<TreeViewVm>();
        services.AddSingleton<CategoryValidator>();
        services.AddSingleton<AddNewCategoryButtonVm>();
        services.AddSingleton<AddCategoryAction>();
        services.AddSingleton<DeleteCategoryButtonVm>();
        services.AddSingleton<DeleteCategoryAction>();
        services.AddSingleton<UpdateCategoryNameButtonVm>();
        services.AddSingleton<UpdateCategoryNameAction>();
        services.AddSingleton<MoveCategoryAction>();
    }
}