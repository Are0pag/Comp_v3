using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class ConfirmSelectionAction : BaseActionAsyncCompletion
{
    protected readonly CounterpartyDataGridVm _dataGridVm;
    protected readonly CounterpartyTableWindow _tableWindow;
    
    public ConfirmSelectionAction(ConfirmSelectiontButVm button, CounterpartyDataGridVm dataGridVm, CounterpartyTableWindow tableWindow) : base(button) {
        _dataGridVm = dataGridVm;
        _tableWindow = tableWindow;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        try {
            EventBus<ICounterpartySubscriber>
               .RaiseEvent<ISelectionConfirmationHandler>(h => h?.OnConfirmSelection(tcs, _dataGridVm.SelectedItem!));
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
        await tcs.Task;
        _tableWindow.Close();
    }

    public override bool CanPerform() {
        return _dataGridVm.SelectedItem != null;
    }
}