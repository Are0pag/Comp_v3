using System.Collections.ObjectModel;
using System.ComponentModel;
using Comp.Db.Contracts;
using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm;

public class SoDataGridVm : DataGridViewModel<SupplierOrder>
{
    protected readonly FiltersVmBase _filtersVm;
    protected readonly SoFilter _filter;
    public SoDataGridVm(IRepository<SupplierOrder> repository, FiltersVmBase filtersVm, SoFilter filter) : base(repository) {
        _filtersVm = filtersVm;
        _filter = filter;
        filtersVm.PropertyChanged += OnFiltersVmOnPropertyChanged;
        
        ItemsSorted = new ObservableCollection<SupplierOrder>(Items);
    }

    public ObservableCollection<SupplierOrder> ItemsSorted {get; set;}
    
    public SupplierOrder? LastSelectedSupplierOrder { get; set; }

    public override SupplierOrder? SelectedItem {
        get => base.SelectedItem;
        set {
            base.SelectedItem = value;
            LastSelectedSupplierOrder = value;
        }
    }

    protected void OnFiltersVmOnPropertyChanged(object? s, PropertyChangedEventArgs e) {
        var comparisonType = _filtersVm.IgnoreCase 
            ? StringComparison.Ordinal 
            : StringComparison.OrdinalIgnoreCase;

        var sorted = Items.Where(item => _filter.ApplyFilter(item, _filtersVm, comparisonType));
        ItemsSorted.Clear();
        foreach (var item in sorted) {
            ItemsSorted.Add(item);
        }
    }

    public void Dispose() {
        _filtersVm.PropertyChanged -= OnFiltersVmOnPropertyChanged;
    }
}