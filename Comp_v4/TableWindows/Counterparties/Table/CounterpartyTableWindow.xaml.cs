using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table;

public partial class CounterpartyTableWindow : Window, IDisposable
{
    public CounterpartyTableWindow(AddCounterpartyButVm addButVm, DataGridVm dataGridVm) {
        InitializeComponent();
        AddButton.DataContext = addButVm;
        MainDataGrid.DataContext = dataGridVm;
    }

    public void Dispose() {
        
    }

    private void CounterpartyTableWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        //EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
    }
}