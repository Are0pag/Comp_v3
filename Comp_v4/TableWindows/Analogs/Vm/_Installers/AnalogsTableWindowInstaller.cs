using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Entities;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using WPF.Services;

namespace Comp_v4.TableWindows.Analogs._Installers;

public class AnalogsTableWindowInstaller : AbstractInstaller
{
    protected readonly AreopagContainer _formContainer = new AreopagContainer() {
        Description = "Analogs form window container created in parent installer [AnalogsTableWindowInstaller]"
    };
    
    protected override void InstallBindings(AreopagContainer container) {
        new AnalogFormInstaller().Install(_formContainer);

        container.Add<IRepository<Analog>>().To<RepoAnalogs>().AsScoped<AnalogsTableWindow>();

        container.Add<AnalogFormManager>()
                 .AsScoped<AnalogsTableWindow>()
                 .UsingFactoryMethod(() => {
                      return new AnalogFormManager(_formContainer);
                  })
                 .EnforceInstantiateOnBegin();

        container.Add<EditTableState>().AsScoped<AnalogsTableWindow>();
        container.Add<Table>()
                 .AsScoped<AnalogsTableWindow>()
                 .UsingFactoryMethod(() => {
                      var initialState = container.Resolve<EditTableState>();
                      var states = new List<BaseTableState>() {
                          initialState,
                      };
                      return new Table(states, initialState);
                  });
        
        container.Add<AnalogsTableVm>().AsScoped<AnalogsTableWindow>();
        container.Add<AddAnalogButtonVm>().AsScoped<AnalogsTableWindow>();

        container.Add<AnalogsTableWindow>().AsTransient();
    }
}