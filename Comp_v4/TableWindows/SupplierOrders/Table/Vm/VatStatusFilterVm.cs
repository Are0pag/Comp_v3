using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm;

public enum VatStatusFilter : byte
{
    [Description("Все")]
    All,
    [Description("Без НДС")]
    WithoutVat,
    [Description("НДС включён")]
    VatIncluded,
    [Description("НДС сверху")]
    VatOnTop
}

public class VatStatusFilterVm : ObservableObject
{
    protected ObservableCollection<SupplierOrder> _items;
    protected ObservableCollection<SupplierOrder> _filteredItems;
    
    public List<VatStatusFilter> VatStatusFilterValues { get; } = new() {
        VatStatusFilter.All,
        VatStatusFilter.WithoutVat,
        VatStatusFilter.VatIncluded,
        VatStatusFilter.VatOnTop
    };
    
    private VatStatusFilter _selectedVatStatusFilter = VatStatusFilter.All;

    public void Init(ObservableCollection<SupplierOrder> items, ObservableCollection<SupplierOrder> filteredItems) {
        _items = items;
        _filteredItems = filteredItems;
    }

    public VatStatusFilter SelectedVatStatusFilter
    {
        get => _selectedVatStatusFilter;
        set {
            _selectedVatStatusFilter = value;
            OnPropertyChanged(nameof(SelectedVatStatusFilter));
            ApplyFilter();
        }
    }

    public void ApplyFilter() {
        _filteredItems.Clear();

        if (SelectedVatStatusFilter == VatStatusFilter.All) {
            foreach (var item in _items) {
                _filteredItems.Add(item);
            }
        }
        else {
            var selectedName = SelectedVatStatusFilter.ToString();

            // Превращаем эту строку в оригинальный бизнес-enum VatStatus
            var targetVatStatus = (VatStatus)Enum.Parse(typeof(VatStatus), selectedName);

            var query = _items.Where(item => item.VatStatusEnumValue == targetVatStatus);
            foreach (var item in query) {
                _filteredItems.Add(item);
            }
        }
    }
}