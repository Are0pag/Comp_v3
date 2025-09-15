using System.ComponentModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.Input;
using Comp.Db.Contracts;
using WPF.Templates.TableWindow.Vm.Components;

namespace WPF.Templates.TableWindow.Vm;

public partial class DataGridViewModelFiltered : DataGridViewModel
{
    private ICollectionView _filteredItems;
    private bool _isCaseSensitive;

    public DataGridViewModelFiltered(IConditionalDesignationRepository repository) : base(repository) {
        DesignationFilter = new DesignationFilter();
        NameFilter = new NameFilter();

        OnItemsChanged();
        
        DesignationFilter.PropertyChanged += OnFilterPropertyChanged;
        NameFilter.PropertyChanged += OnFilterPropertyChanged;
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
            ApplyFilters();
        }
    }

    /// Метод для обновления фильтров после изменений в коллекции
    public void RefreshFilters() {
        ApplyFilters();
    }
    
    [RelayCommand]
    public void ClearFilters() {
        DesignationFilter.FilterText = string.Empty;
        NameFilter.FilterText = string.Empty;
    }

    protected override void OnItemsChanged() {
        base.OnItemsChanged();

        // Инициализируем FilteredItems после загрузки данных
        _filteredItems = CollectionViewSource.GetDefaultView(Items);
        _filteredItems.Filter = CombinedFilter;
        OnPropertyChanged(nameof(FilteredItems));
    }

    private bool CombinedFilter(object item) {
        return DesignationFilter.Filter(item) && NameFilter.Filter(item);
    }

    private void OnFilterPropertyChanged(object sender, PropertyChangedEventArgs e) {
        ApplyFilters();
    }

    private void ApplyFilters() {
        _filteredItems?.Refresh();
    }
}