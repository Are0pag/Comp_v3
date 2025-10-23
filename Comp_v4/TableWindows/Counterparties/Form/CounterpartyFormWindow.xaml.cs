using System.Windows;
using Comp_v4.TableWindows.Counterparties.Form.Vm;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp.ModelData;

namespace Comp_v4.TableWindows.Counterparties;

public partial class CounterpartyFormWindow : Window, IDisposable
{
    public CounterpartyFormWindow(Counterparty counterparty, SaveButVm saveButVm, CounterpartyEnumsVm counterpartyEnumsVm) {
        InitializeComponent();
        
        //CounterpartyTypeComboBox.ItemsSource = Enum.GetValues(typeof(CounterpartyType)).Cast<CounterpartyType>();
        CounterpartyTypeComboBox.DataContext = counterpartyEnumsVm;
        
        DataContext = counterparty;
        SaveButton.DataContext = saveButVm;
    }

    public void Dispose() {
        
    }
}