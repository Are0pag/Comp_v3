using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp_v4.TableWindows.Counterparties.Table;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Templates.Common.Actions;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Form.Actions;

public class SaveCpFormAction : BaseActionAsyncScopeHandler
{
    protected readonly Counterparty _counterparty;
    protected readonly CounterpartyFormWindow _formWindow;
    protected readonly FormCp _formCp;
    protected readonly CounterpartyDataGridVm _dataGridVm;

    public SaveCpFormAction(SaveCpFormButVm button, IServiceScopeFactory scopeFactory, Counterparty counterparty, CounterpartyFormWindow formWindow, FormCp formCp, CounterpartyDataGridVm dataGridVm) 
        : base(button, scopeFactory) {
        _counterparty = counterparty;
        _formWindow = formWindow;
        _formCp = formCp;
        _dataGridVm = dataGridVm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _formCp.Save(new TaskCompletionSource<Counterparty>(), _counterparty);
        await _dataGridVm.Save(new TaskCompletionSource<Counterparty>(), _counterparty);
        EventBus<ICounterpartySubscriber>.
            RaiseEvent<ICpFormOnSaveUiChangesHandler>(h => {
                h?.OnSaveCpForm(new TaskCompletionSource());
            });
        _formWindow.Close();
        tcs.SetResult();
    }

    public override bool CanPerform() {
        return true;
    }
}