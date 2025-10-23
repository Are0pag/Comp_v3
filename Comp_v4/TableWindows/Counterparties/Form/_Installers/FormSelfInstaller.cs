using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp.ModelData;
using DI;
using DI.Contracts;

namespace Comp_v4.TableWindows.Counterparties.Form._Installers;

public class FormSelfInstaller : ISelfLayerInstaller
{
    public AreopagContainer InstallSelf(AreopagContainer selfContainer) {

        selfContainer.Add<Counterparty>()
                     .AsScoped<CounterpartyFormWindow>();
        
        selfContainer.Add<Form.Entities.Form>()
                     .AsScoped<CounterpartyFormWindow>()
                     .EnforceInstantiateOnBegin();

        selfContainer.Add<EditFormState>()
                     .AsScoped<CounterpartyFormWindow>();

        selfContainer.Add<CreateFormState>()
                     .AsScoped<CounterpartyFormWindow>();

        selfContainer.Add<SaveButVm>()
                     .AsScoped<CounterpartyFormWindow>();
        
        selfContainer.Add<SaveAction>()
                     .AsScoped<CounterpartyFormWindow>()
                     .EnforceInstantiateOnBegin();

        selfContainer.Add<CounterpartyFormWindow>()
                     .AsTransient();
        
        return selfContainer;
    }
}