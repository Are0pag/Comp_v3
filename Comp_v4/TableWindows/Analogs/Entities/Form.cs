using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.View;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.Db.Contracts;
using Comp.ModelData;
using Comp.ModelData.Comp;
using Infrastructure.StateMachine;
using Utils.EventBus;
using Utils.WPF;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Entities;

public class Form : GenericStateMachine<BaseFormState, Form>, ISelectAnalogHandler, ISaveHandler
{
    public Form(IEnumerable<BaseFormState> states, BaseFormState initialState) : base(states, initialState) {
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public async Task Save(TaskCompletionSource tcs, Analog analog) {
        await CurrentState.Save(this);
        tcs.SetResult();
    }

    public async Task OnStartSelectingAnalog(object? parameter = null) {
        await CurrentState.OnStartSelectingAnalog(parameter);
    }
}

public abstract class BaseFormState : StateBase<Form>
{
    public abstract Task OnStartSelectingAnalog(object? parameter = null);

    public abstract Task Save(Form form);
}

public class AddFormState : BaseFormState
{
    protected readonly IRepository<Analog> _analogRepository;
    protected readonly Analog _analog;
    protected readonly IWindowOrderLocator _windowOrderLocator;

    public AddFormState(Analog analog, IWindowOrderLocator windowOrderLocator, IRepository<Analog> analogRepository) {
        _analog = analog;
        _windowOrderLocator = windowOrderLocator;
        _analogRepository = analogRepository;
    }

    public override async Task OnStartSelectingAnalog(object? parameter = null) {
        if (parameter is not TaskCompletionSource<Component> completionSource)
            throw new ArgumentException("parameter must be a TaskCompletionSource");
        
        EventBus<INomDictWindowSubscriber>.RaiseEvent<IGridSelectingStateHandler>(h => h?.OnSelecting(completionSource));
        _analog.RelatedComponent = await completionSource.Task;
        _windowOrderLocator.MoveToBack<NomDictWindow>();
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(h => h?.NotifyCanExecute());
    }

    public override async Task Save(Form form) {
        try {
            await _analogRepository.AddAsync(_analog);
        }
        catch (Exception ex) {
            throw;
        }
    }
}


public class EditFormState : BaseFormState
{
    public override async Task OnStartSelectingAnalog(object? parameter = null) {
        throw new NotImplementedException();
    }

    public override async Task Save(Form form) {
        throw new NotImplementedException();
    }
}
