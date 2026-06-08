using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm;

public class VatStatusFilterVm : ObservableObject
{
    protected readonly SoDataGridVm _soDataGridVm;
    
    public List<VatStatus?> VatStatusFilterValues { get; } = new() {
        null, // Это пункт "Все"
        VatStatus.WithoutVat,
        VatStatus.VatIncluded,
        VatStatus.VatOnTop
    };
    
    private VatStatus? _selectedVatStatusFilter;

    public VatStatusFilterVm(SoDataGridVm soDataGridVm) {
        _soDataGridVm = soDataGridVm;
    }

    public VatStatus? SelectedVatStatusFilter
    {
        get => _selectedVatStatusFilter;
        set {
            _selectedVatStatusFilter = value;
            OnPropertyChanged(nameof(SelectedVatStatusFilter));
            ApplyFilter();
        }
    }

    public void ApplyFilter() {
        _soDataGridVm.ItemsSorted.Clear();

        // Фильтруем данные через обычный LINQ в зависимости от выбора
        IEnumerable<SupplierOrder> query;

        if (SelectedVatStatusFilter == null) {
            query = _soDataGridVm.Items; // Если выбрано "Все", берем весь список
        }
        else {
            // Фильтруем по вашему свойству Enum в модели
            query = _soDataGridVm.Items
                                 .Where(item => item.VatStatusEnumValue == SelectedVatStatusFilter.Value);
        }

        foreach (var item in query) {
            _soDataGridVm.ItemsSorted.Add(item);
        }
    }
}