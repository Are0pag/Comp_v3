using Comp_v4.TableWindows.TypeSizes;
using Comp_v4.TableWindows.TypeSizes.Entities.Form;
using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Vm;
using Comp_v4.TableWindows.TypeSizes.Vm.Buttons;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

namespace Comp_v4._Installers.ServiceCollectionExtentions;

public static class TypeSizesInstExt
{
    public static void RegisterTypeSizes(this IServiceCollection services) {
        RegisterForm(services);
        RegisterTable(services);
    }

    private static void RegisterTable(IServiceCollection services) {
        services.AddSingleton<ActionStartAddingNewItem<TypeSizesTableWindow, TypeSize>, ActionAddingNewTs>();
    }

    private static void RegisterForm(IServiceCollection services) {
        services.AddSingleton<ActionSaveNewTsForm>();
        services.AddSingleton<ButtonSaveNewItemForm>();
        
        services.AddSingleton<TsImageFieldVm>();
        services.AddSingleton<SelectTypeSizeImageAction>();
        services.AddSingleton<OpenTsImageAction>();
        services.AddSingleton<ClearTsImageAction>();
        
        States();

        services.AddTransient<TsFormWindow>();

        void States() {
            services.AddSingleton<AddItemTsStateForm>();
            services.AddSingleton<EditItemTsStateForm>();
        
            services.AddSingleton<BaseTsStateForm, EditItemTsStateForm>();
            services.AddSingleton<BaseTsStateForm, AddItemTsStateForm>();
        
            services.AddSingleton<FormTs>();
        }
    }
}