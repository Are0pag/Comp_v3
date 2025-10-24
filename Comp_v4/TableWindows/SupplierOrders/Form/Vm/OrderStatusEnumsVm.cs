using Comp.ModelData;
using Utils.WPF;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm;

public class OrderStatusEnumsVm : EnumVmSourceChanging<OrderStatus, SupplierOrder>
{
    public OrderStatusEnumsVm(SupplierOrder source) : base(source) {
        _selectedValue = OrderStatus.Created;
    }

    public override OrderStatus SelectedValue {
        get => _selectedValue;
        set {
            SetProperty(ref _selectedValue, value);
            _source.OrderStatus = value.ToString();
        }
    }
}