/*using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

public class TargetViewModel : INotifyPropertyChanged
{
    private string _designationFilter;
    private ICollectionView _filteredItems;
    private bool _isCaseSensitive;
    private ObservableCollection<Item> _items;
    private string _nameFilter;

    public TargetViewModel() {
        // Инициализация коллекции
        Items = new ObservableCollection<Item>();
        FilteredItems = CollectionViewSource.GetDefaultView(Items);
        FilteredItems.Filter = FilterItems;

        // Команда очистки фильтров
        ClearFiltersCommand = new RelayCommand(ClearFilters);
    }

    public ObservableCollection<Item> Items {
        get => _items;
        set {
            _items = value;
            OnPropertyChanged();
        }
    }

    public ICollectionView FilteredItems {
        get => _filteredItems;
        set {
            _filteredItems = value;
            OnPropertyChanged();
        }
    }

    public bool IsCaseSensitive {
        get => _isCaseSensitive;
        set {
            _isCaseSensitive = value;
            OnPropertyChanged();
            FilteredItems.Refresh();
        }
    }

    public string DesignationFilter {
        get => _designationFilter;
        set {
            _designationFilter = value;
            OnPropertyChanged();
            FilteredItems.Refresh();
        }
    }

    public string NameFilter {
        get => _nameFilter;
        set {
            _nameFilter = value;
            OnPropertyChanged();
            FilteredItems.Refresh();
        }
    }

    public ICommand ClearFiltersCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    private bool FilterItems(object obj) {
        if (obj is not Item item) return false;

        // Проверяем, есть ли какие-либо фильтры
        var hasFilters = !string.IsNullOrEmpty(IdFilter) ||
                         !string.IsNullOrEmpty(DesignationFilter) ||
                         !string.IsNullOrEmpty(NameFilter);

        // Если фильтров нет, показываем все элементы
        if (!hasFilters) return true;

        var comparison = IsCaseSensitive
            ? StringComparison.CurrentCulture
            : StringComparison.CurrentCultureIgnoreCase;

        // Проверяем фильтр для Id
        var idMatches = string.IsNullOrEmpty(IdFilter) ||
                        item.Id.ToString().Contains(IdFilter, comparison);

        // Проверяем фильтр для Designation
        var designationMatches = string.IsNullOrEmpty(DesignationFilter) ||
                                 (item.Designation != null && item.Designation.Contains(DesignationFilter, comparison));

        // Проверяем фильтр для Name
        var nameMatches = string.IsNullOrEmpty(NameFilter) ||
                          (item.Name != null && item.Name.Contains(NameFilter, comparison));

        // Элемент должен соответствовать всем заданным фильтрам
        return idMatches && designationMatches && nameMatches;
    }

    private void ClearFilters() {
        IdFilter = string.Empty;
        DesignationFilter = string.Empty;
        NameFilter = string.Empty;
    }
}

// Класс Item
public class Item
{
    public int Id { get; set; }
    public string Designation { get; set; }
    public string Name { get; set; }
}

// Extension метод для поиска с учетом сравнения
public static class StringExtensions
{
    public static bool Contains(this string source, string toCheck, StringComparison comp) {
        return source?.IndexOf(toCheck, comp) >= 0;
    }
}*/