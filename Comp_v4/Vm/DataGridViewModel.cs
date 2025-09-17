using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using Utils.WPF.VmEnumerableInteractiveData;
using WPF.Templates.TableWindow.Events;

namespace WPF.Templates.TableWindow.Vm;

public class DataGridViewModel : VmEnumerableInteractiveData<ConditionalDesignation>
{
    protected readonly IRepository<ConditionalDesignation> _repository;
    
    public DataGridViewModel(IRepository<ConditionalDesignation> repository) {
        _repository = repository;
        LoadDataAsync();
    }

    public override ConditionalDesignation? SelectedItem { 
        get => _selectedItem;
        set {
            _selectedItem = value;
            EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
            OnPropertyChanged();
        }
    }

    private async void LoadDataAsync() {  /* VmRepo : базы */
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<ConditionalDesignation?>(items!);
        OnPropertyChanged(nameof(Items));
    }
}