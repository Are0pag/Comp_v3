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
        services.AddScoped<SupplierOrderTableWindow>();
        
        /* form */
        /*services.AddScoped<ISoFormFactory, SoFormFactory>();
        services.AddScoped<EditSoFormState>();
        services.AddScoped<CreateSoFormState>();
        services.AddScoped<BaseSoFormState, EditSoFormState>();
        services.AddScoped<BaseSoFormState, CreateSoFormState>();*/

        services.AddScoped<SupplierOrder>(_ => new SupplierOrder());
        
        services.AddScoped<SaveButVm>();
        services.AddScoped<SaveAction>();
        services.AddScoped<CounterpartySelectButVm>();
        //services.AddScoped<CounterpartySelectAction>();

        services.AddScoped<OrderStatusEnumsVm>();
        services.AddScoped<VatStatusEnumVm>();
        
        services.AddScoped<SupplierOrderFormWindow>();
    }
}