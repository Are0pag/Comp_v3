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
        /*container.Add<IRepository<Analog>>().To<RepoAnalogs>().AsTransient();
        container.Add<IWindowOrderLocator>().To<WindowOrderLocator>().AsSingleton();*/
        container.Add<Analog>().AsScoped<AnalogsFormWindow>();
        container.Add<Component>().AsScoped<AnalogsFormWindow>();

        container.Add<AnalogsForm>().AsScoped<AnalogsFormWindow>().EnforceInstantiateOnBegin();
        container.Add<AddAnalogsFormState>().AsScoped<AnalogsFormWindow>();
        container.Add<EditAnalogsFormState>().AsScoped<AnalogsFormWindow>();
        
        container.Add<SaveAnalogButVm>().AsScoped<AnalogsFormWindow>();
        container.Add<ActionAnalogsSave>().AsScoped<AnalogsFormWindow>().EnforceInstantiateOnBegin();
        
        container.Add<SelectAnalogButtonVm>().AsScoped<AnalogsFormWindow>();
        
        container.Add<AnalogsFormWindow>().AsTransient();
    }
}