/*using Comp_v4.Installers;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Table.Actions;
using Comp_v4.TableWindows.Counterparties.Table.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using DI;
using DI.Contracts;

namespace Comp_v4.TableWindows.Counterparties.Table._Installers;

public class SelfInstaller : ISelfLayerInstaller
{
    public AreopagContainer InstallSelf(AreopagContainer selfContainer) {
        if (selfContainer is not CounterpartyTableContainer)
            throw new ApplicationException("This is not a Counterparty table container.");
        
        selfContainer.Add<ICounterpartyFormHandler>()
                     .To<FormContextInstaller>()
                     .AsScoped<CounterpartyTableWindow>();

        selfContainer.Add<DataGridVm>()
                     .AsScoped<CounterpartyTableWindow>();
        
        selfContainer.Add<AddCounterpartyButVm>().AsScoped<CounterpartyTableWindow>();
        selfContainer.Add<EditCounterpartyButVm>().AsScoped<CounterpartyTableWindow>();
        
        selfContainer.Add<AddAction>()
                     .AsScoped<CounterpartyTableWindow>()
                     .EnforceInstantiateOnBegin();
        
        selfContainer.Add<EditCounterpartyAction>()
                     .AsScoped<CounterpartyTableWindow>()
                     .EnforceInstantiateOnBegin();
        
        selfContainer.Add<Entities.Table>()
                     .AsScoped<CounterpartyTableWindow>()
                     .UsingFactoryMethod(() => {
                          var initialState = selfContainer.Resolve<EditTableState>();
                          var states = new List<BaseTableState>() {
                              initialState,
                          };
                          return new Entities.Table(states, initialState);
                      })
                     .EnforceInstantiateOnBegin();

        selfContainer.Add<EditTableState>()
                     .AsScoped<CounterpartyTableWindow>();

        selfContainer.Add<CounterpartyTableWindow>()
                     .AsTransient();
        
        return selfContainer;
    }
}*/