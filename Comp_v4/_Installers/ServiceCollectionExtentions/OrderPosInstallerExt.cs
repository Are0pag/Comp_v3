using Comp_v4.TableWindows.OrderPositions.Table;
using Comp_v4.TableWindows.OrderPositions.Table.Vm;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4._Installers.ServiceCollectionExtentions;

public static class OrderPosInstallerExt
{
    public static void RegisterOrderPositions(this IServiceCollection services) {
        Table(services);
    }

    private static void Table(IServiceCollection services) {
        services.AddSingleton<OpDataGridVm>();

        services.AddTransient<OrderPositionsTableWindow>();
    }
}