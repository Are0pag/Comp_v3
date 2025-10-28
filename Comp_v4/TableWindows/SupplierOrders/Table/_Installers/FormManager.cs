/*using Comp_v4.Installers;
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp.ModelData;
using DI;
using Utils.EventBus;

namespace Comp_v4.TableWindows.SupplierOrders.Table._Installers;

public class FormManager : IFormHandler
{
    protected readonly SupplierOrderFormContainer _container;

    public FormManager(SupplierOrderFormContainer supplierOrderFormContainer) {
        _container = supplierOrderFormContainer;
        EventBus<ISupplierOrdersSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<ISupplierOrdersSubscriber>.Unsubscribe(this);
    }

    public async Task OpenForm<T>(TaskCompletionSource tcs, object? parameter = null) where T : BaseFormState {
        if (parameter is not SupplierOrder supplierOrder)
            throw new InvalidCastException("Supplier order is not a SupplierOrder");

        _container.SetFactoryMethodFor<SupplierOrder>(() => supplierOrder);

        _container.SetFactoryMethodFor<Form.Entities.Form>(() => {
            var initialState = _container.Resolve<T>();
            var states = new List<BaseFormState>() {
                _container.Resolve<CreateFormState>(),
                _container.Resolve<EditFormState>()
            };
            return new Form.Entities.Form(states, initialState);
        });

        WindowContextResolver.ResolveWindow<SupplierOrderFormWindow>(_container);

        tcs.TrySetResult();
    }
}*/

using Comp_v4.TableWindows.SupplierOrders.Form;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.SupplierOrders.Table._Installers;

public interface ISupplierOrderFormWindowFactory
{
    SupplierOrderFormWindow Create(SupplierOrder order);
}


public class SupplierOrderFormWindowFactory : ISupplierOrderFormWindowFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SupplierOrderFormWindowFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public SupplierOrderFormWindow Create(SupplierOrder order) {
        return ActivatorUtilities.CreateInstance<SupplierOrderFormWindow>(_serviceProvider, order);
    }
}

public interface ISoFormFactory
{
    SoForm Create<TInitialState>() where TInitialState : BaseSoFormState;
}

public class SoFormFactory : ISoFormFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SoFormFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public SoForm Create<TInitialState>() where TInitialState : BaseSoFormState {
        var initialState = _serviceProvider.GetService<TInitialState>();
        var states =  _serviceProvider.GetServices<BaseSoFormState>().ToList();
        return new SoForm(states, initialState!);
    }
}
