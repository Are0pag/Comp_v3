using Comp.ModelData;
using Utils.WPF;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm;

public class VatStatusEnumVm : EnumVmSourceChanging<VatStatus, SupplierOrder>
{
    public VatStatusEnumVm(SupplierOrder source) : base(source) {
        _selectedValue = VatStatus.VatIncluded;
    }

    public override VatStatus SelectedValue {
        get => _selectedValue;
        set {
            SetProperty(ref _selectedValue, value);
            _source.VatStatus = value.ToString();
        }
    }
}