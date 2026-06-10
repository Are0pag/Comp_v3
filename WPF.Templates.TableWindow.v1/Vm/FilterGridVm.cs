using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Comp.Db.Contracts;
using Comp.ModelData.Contracts;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace WPF.Templates.TableWindow.v1.Vm;

public abstract class FilterGridVm<T> : DataGridViewModel<T> where T : class, IDbEntity, IDisposable
{
    protected FiltersVmBase _filtersVm;
    protected readonly IFilter<T, FiltersVmBase> _filter;
    
    public FilterGridVm(IRepository<T> repository, IFilter<T, FiltersVmBase> filter) : base(repository) {
        _filter = filter;
        Items.CollectionChanged += ItemsOnCollectionChanged;
    }

    private void ItemsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        if (e.NewItems == null)
            return;
        foreach (var item in e.NewItems) {
            if (item is not T itemAsT || !_filter.ApplyFilter(itemAsT, _filtersVm, GetComparison()))
                continue;
            ItemsSorted.Add(itemAsT);
            OnPropertyChanged(nameof(ItemsSorted));
        }
    }

    public abstract Task InitFilteringCollection();

    public ObservableCollection<T> ItemsSorted {get; set;}

    public FiltersVmBase FiltersVm {
        get => _filtersVm;
        set {
            _filtersVm = value;
            _filtersVm.PropertyChanged += OnFiltersVmOnPropertyChanged;
        }
    }

    public bool RemoveItem(T item) => ItemsSorted.Remove(item) && Items.Remove(item);

    protected void OnFiltersVmOnPropertyChanged(object? s, PropertyChangedEventArgs e) {
        var comparisonType = GetComparison();
        var sorted = Items.Where(item => _filter.ApplyFilter(item, FiltersVm, comparisonType));
        ItemsSorted.Clear();
        foreach (var item in sorted) {
            ItemsSorted.Add(item);
        }
    }

    private StringComparison GetComparison() {
        var comparisonType = _filtersVm.IgnoreCase 
            ? StringComparison.Ordinal 
            : StringComparison.OrdinalIgnoreCase;
        return comparisonType;
    }

    public void Dispose() {
        _filtersVm.PropertyChanged -= OnFiltersVmOnPropertyChanged;
        Items.CollectionChanged -= ItemsOnCollectionChanged;
    }
}