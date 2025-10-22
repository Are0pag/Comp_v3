using Comp_v4.TableWindows.SupplierOrders.Form.Entities;

namespace Comp_v4.TableWindows.SupplierOrders.Events;

public interface ISupplierOrdersSubscriber : IDisposable { }

public interface IFormHandler : ISupplierOrdersSubscriber
{
    Task OpenForm<T>(TaskCompletionSource tcs, object? parameter = null) where T : BaseFormState;
}

public interface ICreateSupplierOrdersHandler : ISupplierOrdersSubscriber
{
    Task OnCreateSupplierOrder(TaskCompletionSource tcs, object parameter = null);
}