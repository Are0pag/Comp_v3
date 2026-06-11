using Comp_v4.Entry;
using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class AddCounterpartyAction : BaseActionAsyncSelfWaiting 
{
    protected readonly IServiceProvider _serviceProvider;
    protected TaskCompletionSource _tcs;
    public AddCounterpartyAction(AddCounterpartyButVm  button, IServiceProvider serviceProvider) : base(button) {
        _serviceProvider = serviceProvider;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _tcs = tcs;
        var counterparty = new Counterparty();
        var window = ActivatorUtilities.CreateInstance<CounterpartyFormWindow>(_serviceProvider, counterparty);
        var parent = _serviceProvider.GetRequiredService<EntryWindow>();
        WindowService.BindChildToParent(parent, window);

        _serviceProvider.GetRequiredService<SaveCpFormAction>().CurrentCounterparty = counterparty;
        _serviceProvider.GetRequiredService<CancelEditCpFormAction>().CurrentCounterparty = counterparty;
        
        var form = _serviceProvider.GetRequiredService<FormCp>();

        window.Closed += (sender, args) => {
            tcs.TrySetResult();
        };
        window.Show();
        await _tcs.Task;
    }
}