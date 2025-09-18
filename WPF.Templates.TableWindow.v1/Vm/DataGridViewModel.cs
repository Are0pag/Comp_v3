using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;
using WPF.Templates.TableWindow.Events;

namespace WPF.Templates.TableWindow.Vm;

public class DataGridViewModel<T> : VmEnumerableInteractiveData<T>
    where T : class
{
    protected readonly IRepository<T> _repository;
    
    public DataGridViewModel(IRepository<T> repository) {
        _repository = repository;
        LoadDataAsync();
    }

    public override T? SelectedItem { 
        get => _selectedItem;
        set {
            _selectedItem = value;
            EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
            OnPropertyChanged();
        }
    }

    private async void LoadDataAsync() {  /* VmRepo : базы */
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<T?>(items!);
        OnPropertyChanged(nameof(Items));
    }
}