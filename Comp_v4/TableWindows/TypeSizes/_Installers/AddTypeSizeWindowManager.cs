using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.ModelData;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Services;

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

    public void OpenTsForm(object? parameter = null) {
        if (parameter is not TypeSize typeSize)
            throw new InvalidCastException();

        _container.SetFactoryMethodFor<TypeSize>(() => typeSize);
        _container.SetFactoryMethodFor<IImageOwner>(() => typeSize);
        
        var window = _container.BeginScope<AddTypeSizeWindow>();
        _container.Instantiate<SelectImageAction>();
        window.Closing += (sender, args) => {
            _container.ReleaseScope<AddTypeSizeWindow>();
        };
        window.Show();
    }
}