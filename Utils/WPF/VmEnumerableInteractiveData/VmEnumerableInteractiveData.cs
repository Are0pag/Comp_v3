using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Utils.WPF.VmEnumerableInteractiveData;

public class VmEnumerableInteractiveData<T> : ObservableObject
{
    public required ObservableCollection<T?> Items { get; set; } /* свойство должно быть обязательно инициализировано при создании объекта */

    private T? _selectedItem;
    public T? SelectedItem { /* база 100% */
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }
}