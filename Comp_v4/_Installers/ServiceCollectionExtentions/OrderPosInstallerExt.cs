using Comp_v4.TableWindows.OrderPositions.Form;
using Comp_v4.TableWindows.OrderPositions.Form.Actions;
using Comp_v4.TableWindows.OrderPositions.Form.Entities;
using Comp_v4.TableWindows.OrderPositions.Form.Vm;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp_v4.TableWindows.OrderPositions.Table;
using Comp_v4.TableWindows.OrderPositions.Table.Actions;
using Comp_v4.TableWindows.OrderPositions.Table.Entities;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4._Installers.ServiceCollectionExtentions;

public static class OrderPosInstallerExt
{
    public static void RegisterOrderPositions(this IServiceCollection services) {
        Table(services);

        Form(services);
    }

    private static void Form(IServiceCollection services) {
        services.AddSingleton<BaseOpFormState, EditOpFormState>();
        services.AddSingleton<BaseOpFormState, CreateOpFormState>(); // default
        services.AddSingleton<OpForm>();
        
        
        services.AddSingleton<ReceiveStatusEnumVm>();
        
        services.AddSingleton<SaveOrderPositionButVm>();
        services.AddSingleton<SaveOrderPositionAction>();
        
        services.AddSingleton<CancelEditingOpAction>();
        services.AddSingleton<CancelEditingOpButVm>();
        
        services.AddTransient<OrderPositionForm>();
        services.AddTransient<OrderPositionValidator>();
    }

    private static void Table(IServiceCollection services) {
        services.AddSingleton<BaseOpState, ChangedOpTableState>();
        services.AddSingleton<BaseOpState, EditOpTableState>();
        services.AddSingleton<OpTable>();

        services.AddSingleton<CreateOrderPosFormButVm>();
        services.AddSingleton<CreateOrderPosAction>();
        
        services.AddSingleton<EditOrderPosFormButVm>();
        services.AddSingleton<EditOrderPosAction>();

        services.AddSingleton<DeleteOrderPositionButVm>();
        services.AddSingleton<DeleteOrderPositionAction>();
        
        services.AddSingleton<IFilter<OrderPosition, FiltersVmBase>, OpFilter>();
        
        services.AddSingleton<OpDataGridVm>();

        services.AddTransient<OrderPositionsTableWindow>();
    }
}