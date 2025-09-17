using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Utils.WPF.VmEnumerableInteractiveData;

public class VmEnumerableInteractiveData<T> : ObservableObject
{
    public required ObservableCollection<T?> Items { get; set; } /* свойство должно быть обязательно инициализировано при создании объекта */

    protected T? _selectedItem;
    public virtual T? SelectedItem { 
        get => _selectedItem;
        set {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }
    
    public void EnsureNotifyItemsChanged() => OnPropertyChanged(nameof(Items));
}
