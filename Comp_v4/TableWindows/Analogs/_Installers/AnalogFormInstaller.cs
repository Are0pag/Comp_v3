using Comp_v4.TableWindows.Analogs.Actions;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Entities;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using Comp.ModelData.Comp;
using DI;
using Utils.WPF;

namespace Comp_v4.TableWindows.Analogs._Installers;

public class AnalogFormInstaller : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<IRepository<Analog>>().To<RepoAnalogs>().AsTransient();
        container.Add<IWindowOrderLocator>().To<WindowOrderLocator>().AsSingleton();
        container.Add<Analog>().AsScoped<FormWindow>();
        container.Add<Component>().AsScoped<FormWindow>();

        container.Add<Form>().AsScoped<FormWindow>().EnforceInstantiateOnBegin();
        container.Add<AddFormState>().AsScoped<FormWindow>();
        container.Add<EditFormState>().AsScoped<FormWindow>();
        
        container.Add<SaveButVm>().AsScoped<FormWindow>();
        container.Add<ActionSave>().AsScoped<FormWindow>().EnforceInstantiateOnBegin();
        
        container.Add<SelectAnalogButtonVm>().AsScoped<FormWindow>();
        
        container.Add<FormWindow>().AsTransient();
    }
}