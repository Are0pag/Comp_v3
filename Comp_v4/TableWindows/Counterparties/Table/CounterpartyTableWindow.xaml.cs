using System.Windows;
using System.Windows.Controls;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;

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
}