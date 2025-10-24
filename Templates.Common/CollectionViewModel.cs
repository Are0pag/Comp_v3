using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;

namespace Templates.Common;

public abstract class CollectionViewModel<T> : VmEnumerableInteractiveData<T>
    where T : class
{
    protected readonly IRepository<T> _repository;
    
    public CollectionViewModel(IRepository<T> repository) {
        _repository = repository;
        LoadDataAsync();
    }

    public override T? SelectedItem { 
        get => _selectedItem;
        set {
            _selectedItem = value;
            EventBus<Utils.WPF.Buttons.IGlobalButtonEvent>
               .RaiseEvent<Utils.WPF.Buttons.INotifyConditionalsChanged>(n => n?.NotifyCanExecute());
            OnPropertyChanged();
        }
    }

    private async void LoadDataAsync() {  /* VmRepo : базы */
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<T?>(items!)!;
        OnPropertyChanged(nameof(Items));
    }
}