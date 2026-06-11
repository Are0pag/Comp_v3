using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Templates.Common;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Windows;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.Counterparties.Table;

public partial class CounterpartyTableWindow : TableWindowBase, IDisposable, ICpFormOnSaveUiChangesHandler, IReloadable, 
                                               IRuntimeParamsResolver<CounterpartyTableWindow>, ISelectionConfirmationHandler
{
    protected readonly AddCounterpartyButVm _addCounterpartyButVm;
    protected readonly EditCounterpartyButVm _editCounterpartyButVm;
    protected readonly DeleteCounterpartyButVm _deleteCounterpartyButVm;
    
    protected readonly ConfirmSelectiontButVm _confirmSelectiontButVm;

    protected TaskCompletionSource? _tcsMouseDoubleClick;
    public CounterpartyTableWindow(AddCounterpartyButVm addButVm, 
                                   EditCounterpartyButVm editCounterpartyButVm, 
                                   DeleteCounterpartyButVm deleteCounterpartyButVm,
                                   
                                   CounterpartyDataGridVm dataGridVm, 
                                   ConfirmSelectiontButVm confirmSelectiontButVm) {
        InitializeComponent();
        
        _addCounterpartyButVm = addButVm;
        _editCounterpartyButVm = editCounterpartyButVm;
        _deleteCounterpartyButVm = deleteCounterpartyButVm;
        
        _confirmSelectiontButVm = confirmSelectiontButVm;

        AddButton.DataContext = addButVm;
        EditButton.DataContext = editCounterpartyButVm;
        DeleteButton.DataContext = deleteCounterpartyButVm;
        
        MainDataGrid.DataContext = dataGridVm;
        
        EventBus<ICounterpartySubscriber>.Subscribe(this);
        EventBus<IGlSubscriber>.Subscribe(this);
        
        var filtersVm = new FiltersVmBase();
        
        FilterTextBox.DataContext = filtersVm;
        IgnoreCaseCheckBox.DataContext = filtersVm;

        Loaded += (_, _) => {
            dataGridVm.FiltersVm = filtersVm;
            _ = dataGridVm.InitFilteringCollection();
        };
    }

    public Task OnSaveCpForm(TaskCompletionSource tcs, object? parameter = null) {
        //OnReload?.Invoke();
        tcs.TrySetResult();
        return Task.CompletedTask;
    }

    public static Action<TaskCompletionSource, object?, MouseButtonEventArgs> OnDoubleClickSelectingItemInTable { get; set; }

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
            
            case Key.F1:
                var cb = VisualHelper.FindVisualChild<CheckBox>((Window)sender);
                cb.IsChecked = !cb.IsChecked;
                break;
            
            // case Key.K:
            // #if DEBUG
            //     OnReload?.Invoke();
            // #endif
            //     break;
        }
    }
    
    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<CounterpartyTableWindow> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        EventBus<ICounterpartySubscriber>.Unsubscribe(this);
        EventBus<IGlSubscriber>.Unsubscribe(this);
    }

    public async Task OnConfirmSelection(TaskCompletionSource tcs, object parameter = null) {
        Close();
        tcs.TrySetResult();
    }

    private void MainDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
        _addCounterpartyButVm.NotifyCanExecute();
        _editCounterpartyButVm.NotifyCanExecute();
        _deleteCounterpartyButVm.NotifyCanExecute();
        
        _confirmSelectiontButVm.NotifyCanExecute();
    }
}

public class CounterpartyTableDoubleClickTaskCompletionSource : TaskCompletionSource {}