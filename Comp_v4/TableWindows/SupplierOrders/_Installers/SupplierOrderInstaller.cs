using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Actions;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table;
using Comp_v4.TableWindows.SupplierOrders.Table.Actions;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.SupplierOrders.Installers;

public static class SupplierOrderInstaller
{
    public static void RegisterSupplierOrders(this IServiceCollection services) {
        RegisterTable(services);
        RegisterForm(services);
    }

    private static void RegisterForm(IServiceCollection services) {
        services.AddSingleton<SupplierOrder>(_ => new SupplierOrder());
        
        services.AddSingleton<SaveFormButVm>();
        services.AddSingleton<SaveFormAction>();

        services.AddSingleton<CounterpartySelectButVm>();
        services.AddSingleton<CounterpartySelectAction>();

        services.AddSingleton<OrderStatusEnumsVm>();
        services.AddSingleton<VatStatusEnumVm>();
        
        services.AddTransient<SupplierOrderFormWindow>();
    }

    private static void RegisterTable(IServiceCollection services) {
        services.AddSingleton<SoDataGridVm>();
        
        services.AddSingleton<AddSoButVm>();
        services.AddSingleton<AddSoAction>();
        
        services.AddSingleton<EditSoButVm>();
        services.AddSingleton<EditSoAction>();
        
        
        /* form */
        services.AddSingleton<SoForm>();

        services.AddSingleton<BaseSoFormState, EditSoFormState>();
        /* GenericStateMachine(IEnumerable<TState> states, TState initialState)
            - initialState будет = CreateSoFormState, 
              т.к. зарегистрирована последней для BaseSoFormState */
        services.AddSingleton<BaseSoFormState, CreateSoFormState>();
        
        services.AddSingleton<CreateSoFormState>();
        services.AddSingleton<EditSoFormState>();
        
        
        services.AddTransient<SupplierOrderTableWindow>();
    }
}