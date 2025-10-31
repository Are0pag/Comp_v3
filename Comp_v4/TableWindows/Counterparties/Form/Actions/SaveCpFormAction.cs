using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
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

    public SaveCpFormAction(BaseButtonAdvanced button, IServiceScopeFactory scopeFactory, Counterparty counterparty, CounterpartyFormWindow formWindow, FormCp formCp) 
        : base(button, scopeFactory) {
        _counterparty = counterparty;
        _formWindow = formWindow;
        _formCp = formCp;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _formCp.Save(new TaskCompletionSource<Counterparty>(), _counterparty);
        _formWindow.Close();
    }

    public override bool CanPerform() {
        return true;
    }
}