using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.TableWindows.Counterparties.Table;

public partial class CounterpartyTableWindow : Window, IDisposable
{
    protected TaskCompletionSource? _tcsMouseDoubleClick;
    public CounterpartyTableWindow(AddCounterpartyButVm addButVm, DataGridVm dataGridVm) {
        InitializeComponent();
        AddButton.DataContext = addButVm;
        MainDataGrid.DataContext = dataGridVm;
    }

    public void Dispose() {
        
    }

    private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        if (_tcsMouseDoubleClick is {Task.IsCompleted: false})
            return;
        
        _tcsMouseDoubleClick = new TaskCompletionSource();
        _ = HandleDoubleClick(sender, e);
    }

    private async Task HandleDoubleClick(object sender, MouseButtonEventArgs e) {
        // Ждем один цикл диспетчеризации
        await Application.Current.Dispatcher.InvokeAsync(() => {
            if (MainDataGrid.SelectedItem is null)
                return;
            EventBus<ICounterpartySubscriber>
               .RaiseEvent<IMouseDoubleClickHandler>(h => h?.OnMouseDoubleClick(_tcsMouseDoubleClick!, sender, e));
        }, DispatcherPriority.Background);
    }
}