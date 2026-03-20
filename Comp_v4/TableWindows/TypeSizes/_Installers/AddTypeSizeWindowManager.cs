using Comp_v4.TableWindows.TypeSizes.Entities.Form;
using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.ModelData;
using Comp.ModelData.TechnicalItems;
using DI;
using Utils.EventBus;

namespace Comp_v4.TableWindows.TypeSizes;

public class AddTypeSizeWindowManager : ITypeSizeFormOpenHandler
{
    protected readonly AreopagContainer _container;

    public AddTypeSizeWindowManager(AreopagContainer typeSizesNewItemWindowContainer) {
        _container = typeSizesNewItemWindowContainer;
        EventBus<ITypeSizesWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ITypeSizesWindowSubscriber>.Unsubscribe(this);
    }

    public void OpenTsForm<T>(object? parameter = null) 
        where T : BaseTsStateForm
    {
        if (parameter is not TypeSize typeSize)
            throw new InvalidCastException();

        _container.SetFactoryMethodFor<TypeSize>(() => typeSize);
        _container.SetFactoryMethodFor<IImageOwner>(() => typeSize);

        _container.SetFactoryMethodFor<FormTs>(() => {
            var initialState = _container.Resolve<T>();
            var states = new List<BaseTsStateForm>() {
                _container.Resolve<AddItemTsStateForm>(),
                _container.Resolve<EditItemTsStateForm>(),
            };
            return new FormTs(states, initialState);
        });
        
        var window = _container.BeginScope<TsFormWindow>();
        window.Closing += (sender, args) => {
            _container.ReleaseScope<TsFormWindow>();
        };
        window.Show();
    }
}