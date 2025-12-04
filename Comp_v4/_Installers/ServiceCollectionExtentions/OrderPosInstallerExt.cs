using Comp_v4.TableWindows.OrderPositions.Form;
using Comp_v4.TableWindows.OrderPositions.Form.Actions;
using Comp_v4.TableWindows.OrderPositions.Form.Vm;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp_v4.TableWindows.OrderPositions.Table;
using Comp_v4.TableWindows.OrderPositions.Table.Actions;
using Comp_v4.TableWindows.OrderPositions.Table.Entities;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4._Installers.ServiceCollectionExtentions;

public static class OrderPosInstallerExt
{
    public static void RegisterOrderPositions(this IServiceCollection services) {
        Table(services);

        Form(services);
    }

    private static void Form(IServiceCollection services) {
        services.AddSingleton<ReceiveStatusEnumVm>();
        
        services.AddSingleton<SelectPositionButVm>();
        services.AddSingleton<SelectPositionAction>();
        
        services.AddTransient<OrderPositionForm>();
    }

    private static void Table(IServiceCollection services) {
        services.AddSingleton<BaseOpState, ChangedOpTableState>();
        services.AddSingleton<BaseOpState, EditOpTableState>();
        services.AddSingleton<OpTable>();

        services.AddSingleton<CreateOrderPosFormButVm>();
        services.AddSingleton<CreateOrderPosAction>();
        
        services.AddSingleton<EditOrderPosFormButVm>();
        
        services.AddSingleton<OpDataGridVm>();

        services.AddTransient<OrderPositionsTableWindow>();
    }
}