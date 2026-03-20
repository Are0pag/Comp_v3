using System.Windows;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Vm;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties;

public partial class CounterpartyFormWindow : Window, IDisposable
{
    public CounterpartyFormWindow(Counterparty counterparty, SaveCpFormButVm saveButVm, CounterpartyEnumsVm counterpartyEnumsVm) {
        InitializeComponent();
        
        CounterpartyTypeComboBox.DataContext = counterpartyEnumsVm;
        
        DataContext = counterparty;
        SaveButton.DataContext = saveButVm;
        //EventBus<ICounterpartySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        //EventBus<ICounterpartySubscriber>.Unsubscribe(this);
    }

    public Task Save(TaskCompletionSource<Counterparty> tcs, object? parameter = null) {
        Close();
        tcs.TrySetResult((Counterparty)parameter!);
        return Task.CompletedTask;
    }
}