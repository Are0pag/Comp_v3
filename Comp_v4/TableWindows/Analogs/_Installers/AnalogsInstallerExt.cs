using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.Analogs._Installers;

public static class AnalogsInstallerExt
{
    public static void RegisterAnalogs(this IServiceCollection services) {
        services.AddSingleton<AnalogsFieldVm>();
        services.AddSingleton<AnalogsFieldButtonVm>();
        services.AddSingleton<OpenAnalogTableAction>();

    }
}