using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Comp.Db.Contracts;
using WPF.Templates.TableWindow.Vm.Components;

namespace WPF.Templates.TableWindow.Vm;

public partial class DataGridViewModelFiltered : DataGridViewModel
{
    protected ICollectionView _filteredItems;
    protected bool _isCaseSensitive;

    public DataGridViewModelFiltered(IConditionalDesignationRepository repository) : base(repository) {
        DesignationFilter = new DesignationFilter();
        NameFilter = new NameFilter();

        DesignationFilter.PropertyChanged += (s, e) => ApplyFilters();
        NameFilter.PropertyChanged += (s, e) => ApplyFilters();
    }

    public ICollectionView FilteredItems => _filteredItems;
    public DesignationFilter DesignationFilter { get; }
    public NameFilter NameFilter { get; }

    public bool IsCaseSensitive {
        get => _isCaseSensitive;
        set {
            if (_isCaseSensitive == value) return;
            _isCaseSensitive = value;
            DesignationFilter.IsCaseSensitive = value;
            NameFilter.IsCaseSensitive = value;
            OnPropertyChanged();
        }
    }

    [RelayCommand]
    private void ClearFilters() {
        DesignationFilter.FilterText = string.Empty;
        NameFilter.FilterText = string.Empty;
    }

    private bool CombinedFilter(object item) {
        return DesignationFilter.Filter(item) && NameFilter.Filter(item);
    }

    private void ApplyFilters() {
        _filteredItems?.Refresh();
    }
}