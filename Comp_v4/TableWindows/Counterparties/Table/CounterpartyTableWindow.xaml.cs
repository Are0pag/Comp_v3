using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Templates.Common;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties.Table;

public partial class CounterpartyTableWindow : Window, IDisposable, ICpFormOnSaveUiChangesHandler, IReloadable
{
    protected readonly ConfirmSelectiontButVm _confirmSelectiontButVm;
    
    protected TaskCompletionSource? _tcsMouseDoubleClick;
    public CounterpartyTableWindow(AddCounterpartyButVm addButVm, EditCounterpartyButVm editCounterpartyButVm, CounterpartyDataGridVm dataGridVm, 
                                   ConfirmSelectiontButVm confirmSelectiontButVm) {
        InitializeComponent();
        _confirmSelectiontButVm = confirmSelectiontButVm;
        
        AddButton.DataContext = addButVm;
        EditButton.DataContext = editCounterpartyButVm;
        MainDataGrid.DataContext = dataGridVm;
        
        EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
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
            case Key.K:
            #if DEBUG
                OnReload?.Invoke();
            #endif
                break;
        }
    }
}

public class CounterpartyTableDoubleClickTaskCompletionSource : TaskCompletionSource {}