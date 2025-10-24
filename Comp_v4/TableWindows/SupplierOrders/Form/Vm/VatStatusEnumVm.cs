using Comp.ModelData;
using Utils.WPF;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm;

public class VatStatusEnumVm : EnumVm<VatStatus>
{
    public VatStatusEnumVm() {
        _selectedValue = VatStatus.VatIncluded;
    }
}