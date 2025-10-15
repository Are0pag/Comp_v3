using Comp_v4.TableWindows.TypeSizes.Entities.Form;
using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
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

    public void OpenTsForm<T>(object? parameter = null) 
        where T : BaseStateForm
    {
        if (parameter is not TypeSize typeSize)
            throw new InvalidCastException();

        _container.SetFactoryMethodFor<TypeSize>(() => typeSize);
        _container.SetFactoryMethodFor<IImageOwner>(() => typeSize);

        _container.SetFactoryMethodFor<Form>(() => {
                       var initialState = _container.Resolve<T>();
                       var states = new List<BaseStateForm>() {
                           _container.Resolve<AddItemStateForm>(),
                           _container.Resolve<EditItemStateForm>(),
                       };
                       return new Form(states, initialState);
                   });
        
        var window = _container.BeginScope<AddTypeSizeWindow>();
        window.Closing += (sender, args) => {
            _container.ReleaseScope<AddTypeSizeWindow>();
        };
        window.Show();
    }
}