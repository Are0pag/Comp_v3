using Comp_v4.TableWindows.Analogs.Entities;
using Comp.ModelData;
using WPF.Services;

namespace Comp_v4.TableWindows.Analogs._Installers;

public class AnalogFormInstaller : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<Analog>().AsScoped<FormWindow>();
        
        container.Add<Form>().AsScoped<FormWindow>();
        container.Add<AddFormState>().AsScoped<FormWindow>();
        container.Add<EditFormState>().AsScoped<FormWindow>();
        
        container.Add<FormWindow>().AsTransient();
    }
}