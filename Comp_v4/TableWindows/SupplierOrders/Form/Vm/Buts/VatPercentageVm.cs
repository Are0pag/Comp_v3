using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;

public partial class VatPercentageVm : ObservableObject
{
    protected SupplierOrder _supplierOrder;

    public VatPercentageVm(SupplierOrder supplierOrder) {
        _supplierOrder = supplierOrder;
    }

    public float VatPercentage {
        get => _supplierOrder.VatPercentage;
        set {
            if (value < 0) value = 0;
            if (value > 100) value = 100;

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
