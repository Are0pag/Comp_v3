using Comp_v4.TableWindows.Analogs.Entities;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Utils.EventBus;
using Utils.WPF;
using WPF.Services;

namespace Comp_v4.TableWindows.Analogs._Installers;

public class AnalogFormManager : IFormOpenHandler
{
    protected readonly AreopagContainer _formContainer;
    protected readonly Component _component;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    
    public AnalogFormManager(AreopagContainer formContainer, Component component, IWindowOrderLocator windowOrderLocator) {
        _formContainer = formContainer;
        _component = component;
        _windowOrderLocator = windowOrderLocator;
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public void OpenForm<T>(object? parameter = null) where T : BaseFormState {
        if (parameter is not Analog analog) 
            throw new ArgumentException();

        analog.SourceComponent ??= _component;
        
        _formContainer.SetFactoryMethodFor<Analog>(() => {
            return analog;
        });
        
        _formContainer.SetFactoryMethodFor<Form>(() => {
            var initialState = _formContainer.Resolve<T>();
            var states = new List<BaseFormState>() {
                _formContainer.Resolve<AddFormState>(),
                _formContainer.Resolve<EditFormState>()
            };
            return new Form(states, initialState);
        });

        var window = _formContainer.BeginScope<FormWindow>();
        _windowOrderLocator.RegisterWindow(window);
        window.Closed += (sender, args) => {
            _formContainer.ReleaseScope<FormWindow>();
        };
        window.Show();
    }
}