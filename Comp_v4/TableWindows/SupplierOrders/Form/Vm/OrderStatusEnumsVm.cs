using Comp.ModelData;
using Utils.WPF;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm;

public class OrderStatusEnumsVm : EnumVm<OrderStatus>
{
    public OrderStatusEnumsVm() {
        _selectedValue = OrderStatus.Created;
    }
}