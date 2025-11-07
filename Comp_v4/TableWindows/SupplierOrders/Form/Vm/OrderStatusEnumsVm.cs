using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm;

public class OrderStatusEnumsVm : EnumVmSourceChanging<OrderStatus, SupplierOrder>, ICreateSupplierOrdersHandler
{
    public OrderStatusEnumsVm(SupplierOrder source) : base(source) {
        _selectedValue = OrderStatus.Created;
        EventBus<ISupplierOrdersSubscriber>.Subscribe(this);
    }

    public override OrderStatus SelectedValue {
        get => _selectedValue;
        set {
            SetProperty(ref _selectedValue, value);
            _source.OrderStatus = value.ToString();
        }
    }

    public void Dispose() {
        EventBus<ISupplierOrdersSubscriber>.Unsubscribe(this);
    }

    public Task OnCreateSupplierOrder(TaskCompletionSource tcs, object parameter = null) {
        SelectedValue = Enum.Parse<OrderStatus>(_source.OrderStatus);
        tcs.TrySetResult();
        return Task.CompletedTask;
    }
}