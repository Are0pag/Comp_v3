using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Actions;

public class AddCounterpartyAction : BaseActionAsyncCompletion<Counterparty>
{
    protected readonly ICounterpartyFormHandler _formHandler;
    protected TaskCompletionSource<Counterparty>? _currentTcs;
    public AddCounterpartyAction(AddCounterpartyButVm button, ICounterpartyFormHandler formHandler) : base(button) {
        _formHandler = formHandler;
    }

    public override async void Perform(TaskCompletionSource<Counterparty> tcs) {
        _currentTcs = tcs;
        var subscriberTcs = new TaskCompletionSource();
        var counterparty = new Counterparty();
        await _formHandler.Open<CreateCpFormState>(subscriberTcs, counterparty);
        await subscriberTcs.Task;
        _currentTcs.TrySetResult(counterparty);
    }

    public override bool CanPerform() {
        return _currentTcs is null || _currentTcs.Task.IsCompleted;
    }
}