using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Form.Actions;

public class SaveCpFormAction : BaseActionAsyncCompletion<Counterparty>
{
    protected readonly Counterparty _counterparty;
    protected readonly CounterpartyFormWindow _formWindow;
    protected readonly CounterpartyDataGridVm _dataGridVm;
    protected readonly FormCp _formCp;
    
    public SaveCpFormAction(SaveCpFormButVm button, Counterparty counterparty, CounterpartyFormWindow formWindow, CounterpartyDataGridVm dataGridVm, FormCp formCp) : base(button) {
        _counterparty = counterparty;
        _formWindow = formWindow;
        _dataGridVm = dataGridVm;
        _formCp = formCp;
    }

    public override async void Perform(TaskCompletionSource<Counterparty> tcs) {
        await _formCp.Save(new TaskCompletionSource<Counterparty>(), _counterparty);
        await _dataGridVm.Save(new TaskCompletionSource<Counterparty>(), _counterparty);

        _formWindow.Close();
    }

    public override bool CanPerform() {
        return true;
    }
}