using System.Collections.ObjectModel;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Utils.WPF.VmEnumerableInteractiveData;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates.TableWindow.Vm;

public class ViewModel : VmEnumerableInteractiveData<ConditionalDesignation>//, IViewModel
{
    protected readonly IConditionalDesignationRepository _repository;
    
    public ViewModel(StateProvider stateProvider, IConditionalDesignationRepository repository) {
        StateProvider = stateProvider;
        _repository = repository;
        
        LoadDataAsync();
    }

    public required StateProvider StateProvider { get; init; }

    private async void LoadDataAsync() {  /* VmRepo : базы */
        var items = await _repository.GetAllAsync();
        Items = new ObservableCollection<ConditionalDesignation?>(items!);
        OnPropertyChanged(nameof(Items));
    }
    
    

    public async Task SaveChanges() {
        await StateProvider.CurrentState.SaveChanges();
    }
}