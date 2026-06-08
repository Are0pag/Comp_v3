using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Events;

public interface ISupplierOrdersSubscriber : IDisposable { }

public interface IFormHandler : ISupplierOrdersSubscriber
{
    Task OpenForm<T>(TaskCompletionSource tcs, object? parameter = null) where T : BaseSoFormState;
}

public interface ICreateSupplierOrdersHandler : ISupplierOrdersSubscriber
{
    Task OnCreateSupplierOrder(TaskCompletionSource tcs, object parameter = null);
}

public interface ISoPropertyChangeHandler : ISupplierOrdersSubscriber
{
    //void OnSoPropertyChanged(Action<SupplierOrder> propertyChange, object parameter = null);
    void OnOrderPositionChanged(object parameter = null);
}