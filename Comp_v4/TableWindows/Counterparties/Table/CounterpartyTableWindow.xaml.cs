using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
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

public partial class CounterpartyTableWindow : TableWindowBase, IDisposable, ICpFormOnSaveUiChangesHandler, IReloadable, IRuntimeParamsResolver<CounterpartyTableWindow>
{
    protected readonly AddCounterpartyButVm _addCounterpartyButVm;
    protected readonly EditCounterpartyButVm _editCounterpartyButVm;
    protected readonly ConfirmSelectiontButVm _confirmSelectiontButVm;
    protected readonly DeleteCounterpartyButVm _deleteCounterpartyButVm;
    private DispatcherTimer _timer;
    
    [DllImport("dwmapi.dll", PreserveSig = true)]
    public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    // Константа, отвечающая за отключение анимаций окна
    private const int DWMWA_TRANSITIONS_FORCED_OFF = 3;
    
    protected TaskCompletionSource? _tcsMouseDoubleClick;
    public CounterpartyTableWindow(AddCounterpartyButVm addButVm, 
                                   EditCounterpartyButVm editCounterpartyButVm, 
                                   DeleteCounterpartyButVm deleteCounterpartyButVm,
                                   
                                   CounterpartyDataGridVm dataGridVm, 
                                   ConfirmSelectiontButVm confirmSelectiontButVm) {
        InitializeComponent();
        InitTimer();
        
        _addCounterpartyButVm = addButVm;
        _editCounterpartyButVm = editCounterpartyButVm;
        _confirmSelectiontButVm = confirmSelectiontButVm;
        _deleteCounterpartyButVm = deleteCounterpartyButVm;
        
        AddButton.DataContext = addButVm;
        EditButton.DataContext = editCounterpartyButVm;
        DeleteButton.DataContext = deleteCounterpartyButVm;
        
        MainDataGrid.DataContext = dataGridVm;
        
        EventBus<ICounterpartySubscriber>.Subscribe(this);
        EventBus<IGlSubscriber>.Subscribe(this);
        
        var filtersVm = new FiltersVmBase();
        dataGridVm.FiltersVm = filtersVm;
        FilterTextBox.DataContext = filtersVm;
        IgnoreCaseCheckBox.DataContext = filtersVm;

        Loaded += (_, _) => {
            _ = dataGridVm.InitFilteringCollection();
        };
        SourceInitialized += MainWindow_SourceInitialized;
    }
    
    private void MainWindow_SourceInitialized(object sender, EventArgs e)
    {
        // Получаем Handle текущего окна WPF
        IntPtr hwnd = new WindowInteropHelper(this).Handle;

        // Значение "1" означает TRUE (принудительно выключить анимации переходов)
        int value = 1; 

        // Отключаем анимацию открытия/закрытия/сворачивания для этого конкретного окна
        DwmSetWindowAttribute(hwnd, DWMWA_TRANSITIONS_FORCED_OFF, ref value, sizeof(int));
    }

    private void InitTimer() {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(200);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e) {
        _addCounterpartyButVm.NotifyCanExecute();
        _editCounterpartyButVm.NotifyCanExecute();
        _deleteCounterpartyButVm.NotifyCanExecute();
        _confirmSelectiontButVm.NotifyCanExecute();
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
        SourceInitialized -= MainWindow_SourceInitialized;
    }
    
}

public class CounterpartyTableDoubleClickTaskCompletionSource : TaskCompletionSource {}