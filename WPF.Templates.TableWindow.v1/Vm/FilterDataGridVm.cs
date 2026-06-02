using System.Collections.ObjectModel;
using System.ComponentModel;
using Comp.Db.Contracts;
using Comp.ModelData.Contracts;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace WPF.Templates.TableWindow.v1.Vm;

public class FilterDataGridVm<T> : DataGridViewModel<T>
    where T : class, IDbEntity
{
    protected readonly FiltersVmBase _filtersVm;
    protected readonly IFilter<T, FiltersVmBase> _filter;
    
    public FilterDataGridVm(IRepository<T> repository, FiltersVmBase filtersVm, IFilter<T, FiltersVmBase> filter) : base(repository) {
        _filtersVm = filtersVm;
        _filter = filter;
        
        filtersVm.PropertyChanged += OnFiltersVmOnPropertyChanged;
        
        ItemsSorted = new ObservableCollection<T>(Items);
    }
    
    public ObservableCollection<T> ItemsSorted {get; set;}
    
    private void OnFiltersVmOnPropertyChanged(object? s, PropertyChangedEventArgs e) {
        var comparisonType = _filtersVm.IgnoreCase 
            ? StringComparison.Ordinal 
            : StringComparison.OrdinalIgnoreCase;

        var sorted = Items.Where(item => _filter.ApplyFilter(item, _filtersVm, comparisonType));
        ItemsSorted.Clear();
        foreach (var item in sorted) {
            ItemsSorted.Add(item);
        }
    }

    public virtual void Dispose() {
        _filtersVm.PropertyChanged -= OnFiltersVmOnPropertyChanged;
    }
}