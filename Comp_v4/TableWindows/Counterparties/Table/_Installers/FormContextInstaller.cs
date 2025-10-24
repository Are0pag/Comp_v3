using Comp_v4.Installers;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp.ModelData;
using DI;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Table._Installers;

public class FormContextInstaller : ICounterpartyFormHandler
{
    protected readonly CounterpartyFormContainer _formContainer;
    
    public FormContextInstaller(CounterpartyFormContainer formContainer) {
        _formContainer = formContainer;
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
    }

    public async Task Open<T>(TaskCompletionSource tcs, object? parameter = null) where T : BaseFormState {
        if (parameter is not Counterparty counterparty)
            throw new ArgumentException($"Parameter is not of type {nameof(CounterpartyFormContainer)}");
        
        _formContainer.SetFactoryMethodFor<Counterparty>(() => counterparty);
        
        _formContainer.SetFactoryMethodFor<Form.Entities.Form>(() => {
            var initialState = _formContainer.Resolve<T>();
            var states = new List<BaseFormState>() {
                _formContainer.Resolve<EditFormState>(),
                _formContainer.Resolve<CreateFormState>(),
            };
            return new Form.Entities.Form(states, initialState);
        });

        var window = WindowContextResolver.ResolveWindow<CounterpartyFormWindow>(_formContainer);
        tcs.SetResult();
    }
}