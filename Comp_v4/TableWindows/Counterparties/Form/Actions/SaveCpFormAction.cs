using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Form.Actions;

public class SaveCpFormAction : BaseActionAsyncCompletion
{
    protected readonly FormCp _formCp;
    protected readonly CounterpartyDataGridVm _dataGridVm;
    protected Counterparty _counterparty;

    public SaveCpFormAction(SaveCpFormButVm button, 
                            FormCp formCp, 
                            CounterpartyDataGridVm dataGridVm) 
        : base(button) {
        _formCp = formCp;
        _dataGridVm = dataGridVm;
    }

    public Counterparty CurrentCounterparty {
        get => _counterparty;
        set => _counterparty = value;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _formCp.Save(new TaskCompletionSource<Counterparty>(), _counterparty);
        await _dataGridVm.Save(new TaskCompletionSource<Counterparty>(), _counterparty);
        tcs.SetResult();
        new InstanceContainer<CounterpartyFormWindow>().RuntimeParam.Close();
    }

    public override bool CanPerform() {
        return _counterparty != null && _counterparty.ShortName != null;
    }
}