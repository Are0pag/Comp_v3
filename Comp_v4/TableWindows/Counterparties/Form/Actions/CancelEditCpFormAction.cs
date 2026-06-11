using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Form.Actions;

public class CancelEditCpFormAction : BaseActionAsyncCompletion
{
    protected readonly CounterpartyDataGridVm _dataGridVm;
    protected Counterparty _counterparty;
    protected Counterparty _origin;
    
    public CancelEditCpFormAction(CancelEditingCpButVm button, CounterpartyDataGridVm dataGridVm) : base(button) {
        _dataGridVm = dataGridVm;
    }

    public Counterparty CurrentCounterparty {
        get => _counterparty;
        set {
            _counterparty = value;
            _origin = new Counterparty().PopulateFrom(value);
        }
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _counterparty.PopulateFrom(_origin);
        tcs.TrySetResult();
        new InstanceContainer<CounterpartyFormWindow>().RuntimeParam.Close();
    }

    public override bool CanPerform() {
        return true;
    }
}