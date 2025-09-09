using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Utils.WPF.VmEnumerableInteractiveData;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates.TableWindow.Vm;

public class DataGridViewModel : VmEnumerableInteractiveData<ConditionalDesignation>
{
    protected readonly IConditionalDesignationRepository _repository;
    
    public DataGridViewModel(IConditionalDesignationRepository repository) {
        _repository = repository;
        
        LoadDataAsync();
    }

    public required StateProvider StateProvider { get; init; }

    private async void LoadDataAsync() {  /* VmRepo : базы */
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<ConditionalDesignation?>(items!);
        OnPropertyChanged(nameof(Items));
    }
}