using Comp_v4.Entry.Events;
using Comp_v4.Installers;
using Comp_v4.NomDict.View;
using DI;
using Infrastructure.StateMachine;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.Entry.Entities;

public class ToolsPanel : GenericStateMachine<BaseToolsPanelState, ToolsPanel>, IOpenNomDictHandler
{
    public ToolsPanel(IEnumerable<BaseToolsPanelState> states, BaseToolsPanelState initialState) : base(states, initialState) {
        EventBus<IEntrySubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IEntrySubscriber>.Unsubscribe(this);
    }

    public void OpenNomDict(TaskCompletionSource tcs, object? arg = null) {
        CurrentState.OpenNomDict(tcs, arg);
    }
}

public abstract class BaseToolsPanelState : StateBase<ToolsPanel>
{
    public abstract void OpenNomDict(TaskCompletionSource tcs, object? o);
}

public class ToolsPanelStateIdle : BaseToolsPanelState
{
    protected readonly NomDictContainer _nomDictContainer;

    public ToolsPanelStateIdle(NomDictContainer nomDictContainer) {
        _nomDictContainer = nomDictContainer;
    }

    public override void OpenNomDict(TaskCompletionSource tcs, object? o) {
        var window = WindowContextResolver.ResolveWindow<NomDictWindow>(_nomDictContainer);
        //var window = subContainer.BeginScope<NomDictWindow>();
        _nomDictContainer.Resolve<IWindowOrderLocator>().RegisterWindow(window);
        /*window.Closed += (_, _) => subContainer.ReleaseScope<NomDictWindow>();
        //_subContainers[typeof(NomDictWindow)].Instantiate<AddCategoryAction, DeleteCategoryAction, UpdateCategoryNameAction, DataGridInputHandler>();
        //_subContainers[typeof(NomDictWindow)].Instantiate<AddComponentAction>();
        window.Show();*/
    }
}