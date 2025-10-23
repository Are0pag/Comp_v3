using System.Windows;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;

namespace Comp_v4.TableWindows.Counterparties.Table;

public partial class CounterpartyTableWindow : Window, IDisposable
{
    public CounterpartyTableWindow(AddCounterpartyButVm addButVm) {
        InitializeComponent();
        AddButton.DataContext = addButVm;
    }

    public void Dispose() {
        
    }
}