using System.Collections.ObjectModel;
using System.ComponentModel;
using Comp_v4._Installers;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm;

public class SoDataGridVm : DataGridViewModel<SupplierOrder>, IRuntimeParamsResolver<SoDataGridVm>
{
    protected readonly FiltersVmBase _filtersVm;
    protected readonly SoFilter _filter;
    public SoDataGridVm(IRepository<SupplierOrder> repository, FiltersVmBase filtersVm, SoFilter filter) : base(repository) {
        _filtersVm = filtersVm;
        _filter = filter;
        filtersVm.PropertyChanged += OnFiltersVmOnPropertyChanged;
        
        ItemsSorted = new ObservableCollection<SupplierOrder>(Items);
        
        EventBus<IGlSubscriber>.Subscribe(this);
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

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<SoDataGridVm> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        _filtersVm.PropertyChanged -= OnFiltersVmOnPropertyChanged;
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }
}