using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm;

public class VatStatusEnumVm : EnumVmSourceChanging<VatStatus, SupplierOrder>, ICreateSupplierOrdersHandler
{
    public VatStatusEnumVm(SupplierOrder source) : base(source) {
        _selectedValue = VatStatus.VatIncluded;
        EventBus<ISupplierOrdersSubscriber>.Subscribe(this);
    }

    public override VatStatus SelectedValue {
        get => _selectedValue;
        set {
            SetProperty(ref _selectedValue, value);
            _source.VatStatus = value.ToString();
        }
    }

    public void Dispose() {
        EventBus<ISupplierOrdersSubscriber>.Unsubscribe(this);
    }

    public Task OnCreateSupplierOrder(TaskCompletionSource tcs, object parameter = null) {
        SelectedValue = Enum.Parse<VatStatus>(_source.VatStatus);
        tcs.TrySetResult();
        return Task.CompletedTask;
    }
}