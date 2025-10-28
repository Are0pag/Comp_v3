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
        /* Table */
        services.AddScoped<SoDataGridVm>();
        
        services.AddScoped<AddSoButVm>();
        services.AddScoped<AddSoAction>();
        
        services.AddScoped<EditSoButVm>();
        services.AddScoped<EditSoAction>();
        
        services.AddScoped<SupplierOrderTableWindow>();
        
        /* form */
        services.AddScoped<SoForm>();

        services.AddScoped<BaseSoFormState, EditSoFormState>();
        /* GenericStateMachine(IEnumerable<TState> states, TState initialState)
            - initialState будет = CreateSoFormState, 
              т.к. зарегистрирована последней для BaseSoFormState */
        services.AddScoped<BaseSoFormState, CreateSoFormState>();
        
        services.AddScoped<CreateSoFormState>();
        services.AddScoped<EditSoFormState>();
        

        services.AddScoped<SupplierOrder>(_ => new SupplierOrder());
        
        services.AddScoped<SaveFormButVm>();
        services.AddScoped<SaveFormAction>();

        services.AddScoped<CounterpartySelectButVm>();
        //services.AddScoped<CounterpartySelectAction>();

        services.AddScoped<OrderStatusEnumsVm>();
        services.AddScoped<VatStatusEnumVm>();
        
        services.AddScoped<SupplierOrderFormWindow>();
    }
}