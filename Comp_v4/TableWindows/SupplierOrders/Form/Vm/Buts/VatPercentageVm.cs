using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;

public partial class VatPercentageVm : ObservableObject
{
    protected readonly SupplierOrder _supplierOrder;
    protected readonly VatStatusEnumVm _vatStatusEnumVm;

    public VatPercentageVm(SupplierOrder supplierOrder, VatStatusEnumVm vatStatusEnumVm) {
        _supplierOrder = supplierOrder;
        _vatStatusEnumVm = vatStatusEnumVm;
    }

    public float VatPercentage {
        get => _supplierOrder.VatPercentage;
        set {
            if (value < 0) value = 0;
            if (value > 100) value = 100;

            if (value == 0) {
                if (_vatStatusEnumVm.SelectedValue != VatStatus.WithoutVat)
                    _vatStatusEnumVm.SelectedValue = VatStatus.WithoutVat;
            }
            else {
                if (_vatStatusEnumVm.SelectedValue == VatStatus.WithoutVat) {
                    _vatStatusEnumVm.SelectedValue = VatStatus.VatIncluded;
                }
            }

            _supplierOrder.VatPercentage = value;
            OnPropertyChanged();
        }
    }

    // Генератор автоматически создаст команду с именем "IncreaseVatCommand"
    [RelayCommand]
    private void IncreaseVat() {
        VatPercentage += 1;
    }

    // Генератор автоматически создаст команду с именем "DecreaseVatCommand"
    [RelayCommand]
    private void DecreaseVat() {
        VatPercentage -= 1;
    }
}
