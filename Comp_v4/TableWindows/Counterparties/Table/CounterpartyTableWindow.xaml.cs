using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Templates.Common;
using Utils.EventBus;
using Utils.WPF.Windows;

namespace Comp_v4.TableWindows.Counterparties.Table;

public partial class CounterpartyTableWindow : TableWindowBase, IDisposable, ICpFormOnSaveUiChangesHandler, IReloadable, IRuntimeParamsResolver<CounterpartyTableWindow>
{
    protected readonly ConfirmSelectiontButVm _confirmSelectiontButVm;
    protected readonly DeleteCounterpartyButVm _deleteCounterpartyButVm;
    
    protected TaskCompletionSource? _tcsMouseDoubleClick;
    public CounterpartyTableWindow(AddCounterpartyButVm addButVm, 
                                   EditCounterpartyButVm editCounterpartyButVm, 
                                   DeleteCounterpartyButVm deleteCounterpartyButVm,
                                   
                                   CounterpartyDataGridVm dataGridVm, 
                                   ConfirmSelectiontButVm confirmSelectiontButVm) {
        InitializeComponent();
        
        _confirmSelectiontButVm = confirmSelectiontButVm;
        _deleteCounterpartyButVm = deleteCounterpartyButVm;
        
        AddButton.DataContext = addButVm;
        EditButton.DataContext = editCounterpartyButVm;
        DeleteButton.DataContext = deleteCounterpartyButVm;
        
        MainDataGrid.DataContext = dataGridVm;
        
        EventBus<ICounterpartySubscriber>.Subscribe(this);
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public Task OnSaveCpForm(TaskCompletionSource tcs, object? parameter = null) {
        //OnReload?.Invoke();
        tcs.TrySetResult();
        return Task.CompletedTask;
    }

    public Action<TaskCompletionSource, object?, MouseButtonEventArgs> OnDoubleClickSelectingItemInTable { get; set; }

    public Func<Task> OnReload { get; set; }

    private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        if (_tcsMouseDoubleClick is {Task.IsCompleted: false})
            return;

        _tcsMouseDoubleClick = new CounterpartyTableDoubleClickTaskCompletionSource();
        _ = HandleDoubleClick(sender, e);
    }

    private async Task HandleDoubleClick(object sender, MouseButtonEventArgs e) {
        // Ждем один цикл диспетчеризации
        await Application.Current.Dispatcher.InvokeAsync(() => {
            if (MainDataGrid.SelectedItem is null)
                return;
            /*EventBus<ICounterpartySubscriber>
               .RaiseEvent<IMouseDoubleClickHandler>(h => h?.OnMouseDoubleClick(_tcsMouseDoubleClick!, sender, e));*/
            OnDoubleClickSelectingItemInTable?.Invoke(_tcsMouseDoubleClick!, MainDataGrid, e);
        }, DispatcherPriority.Background);
    }

    private void CounterpartyTableWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Enter when Keyboard.Modifiers == ModifierKeys.Shift:
                if (_confirmSelectiontButVm.IsEnabled)
                    _confirmSelectiontButVm.OnClickAsync();
                break;
            
            case Key.Delete:
                if (_deleteCounterpartyButVm.CanClick())
                    _deleteCounterpartyButVm.OnClickAsync();
                break;
            
            case Key.K:
            #if DEBUG
                OnReload?.Invoke();
            #endif
                break;
        }
    }
    
    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<CounterpartyTableWindow> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }
    
}

public class CounterpartyTableDoubleClickTaskCompletionSource : TaskCompletionSource {}