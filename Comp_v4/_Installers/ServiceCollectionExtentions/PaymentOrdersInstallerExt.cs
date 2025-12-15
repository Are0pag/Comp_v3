using Comp_v4.TableWindows.PaymentOrders.Form.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table;
using Comp_v4.TableWindows.PaymentOrders.Table.Actions;
using Comp_v4.TableWindows.PaymentOrders.Table.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4._Installers.ServiceCollectionExtentions;

public static class PaymentOrdersInstallerExt
{
    public static void RegisterPaymentOrders(this IServiceCollection services) {
        Table(services);
        Form(services);
    }

    private static void Form(IServiceCollection services) {
        services.AddSingleton<CreatePoState>();
        services.AddSingleton<EditPoState>();
        services.AddSingleton<PaymentOrderFormBaseState, CreatePoState>();
        services.AddSingleton<PaymentOrderFormBaseState, EditPoState>();
        services.AddSingleton<PaymentOrderForm>();
        
        services.AddSingleton<SavePaymentOrderButVm>();
    }

    private static void Table(IServiceCollection services) {
        services.AddSingleton<PaymentOrderTableInitialState>();
        services.AddSingleton<PaymentOrderTableBaseState, PaymentOrderTableInitialState>();
        services.AddSingleton<PaymentOrderTable>();

        services.AddSingleton<AddPoAction>();
        
        services.AddSingleton<PaymentOrdersGridVm>();
        
        services.AddSingleton<AddPaymentOrderButVm>();
        services.AddSingleton<EditPaymentOrderButVm>();
        services.AddSingleton<DeletePaymentOrderButVm>();

        services.AddTransient<PaymentOrdersTableWindow>();
    }
}