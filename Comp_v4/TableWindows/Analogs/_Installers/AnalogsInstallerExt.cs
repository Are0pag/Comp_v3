using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Comp_v4.TableWindows.Analogs.Actions;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.Analogs._Installers;

public static class AnalogsInstallerExt
{
    public static void RegisterAnalogs(this IServiceCollection services) {
        Table(services);

        Form(services);
    }

    private static void Form(IServiceCollection services) {
        States();

        services.AddSingleton<SaveAnalogButVm>();
        services.AddSingleton<ActionAnalogsSave>();

        services.AddTransient<AnalogsFormWindow>();
        
        void States() {
            services.AddSingleton<EditAnalogsFormState>();
            services.AddSingleton<AddAnalogsFormState>();
        
            services.AddSingleton<BaseAnalogsFormState, EditAnalogsFormState>();
            services.AddSingleton<BaseAnalogsFormState, AddAnalogsFormState>();

            services.AddSingleton<AnalogsForm>();
        }
    }

    private static void Table(IServiceCollection services) {
        services.AddSingleton<AnalogsFieldVm>();
        services.AddSingleton<AnalogsFieldButtonVm>();
        services.AddSingleton<OpenAnalogTableAction>();
        
        services.AddSingleton<AnalogsTableVm>();
        services.AddSingleton<AddAnalogButtonVm>();

        services.AddTransient<AnalogsTableWindow>();
    }
}