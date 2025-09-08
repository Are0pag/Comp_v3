using System.Windows.Controls;
using System.Windows.Input;
using Comp.ModelData.TechnicalItems;
using Utils.WPF.VmEnumerableInteractiveData;
using WPF.Templates.Core;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates.TableWindow.Vm;

public class ViewModel : VmEnumerableInteractiveData<ConditionalDesignation>, IViewModel
{
    public ViewModel(StateProvider stateProvider) {
        StateProvider = stateProvider;
    }

    public required StateProvider StateProvider { get; init; }

    public async Task AddItemAsync(ViewModel context) {
        await StateProvider.CurrentState.AddItemAsync(context);
    }

    public async Task DeleteItemAsync(ViewModel context) {
        await StateProvider.CurrentState.DeleteItemAsync(context);
    }

    public async Task EditItemAsync(ViewModel context) {
        await StateProvider.CurrentState.EditItemAsync(context);
    }

    public async Task SaveChanges() {
        await StateProvider.CurrentState.SaveChanges();
    }

    public async Task OnCellEditEnding(ViewModel vm, object? sender, DataGridCellEditEndingEventArgs e) {
        await StateProvider.CurrentState.OnCellEditEnding(vm, sender, e);
    }

    public async Task OnHandleKeyInput(ViewModel vm, object? sender, KeyEventArgs e) {
        await StateProvider.CurrentState.OnHandleKeyInput(vm, sender, e);
    }

    public async Task CancelNewItemAdding() {
        await StateProvider.CurrentState.CancelNewItemAdding();
    }
}