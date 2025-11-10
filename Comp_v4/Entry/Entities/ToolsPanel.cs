using Comp_v4.Entry.Events;
using Comp_v4.Installers;
using Comp_v4.NomDict.View;
using DI;
using Infrastructure.StateMachine;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.Entry.Entities;

public class ToolsPanel : GenericStateMachine<BaseToolsPanelState, ToolsPanel>
{
    public ToolsPanel(IEnumerable<BaseToolsPanelState> states, BaseToolsPanelState initialState) : base(states, initialState) {
        
    }

    public void Dispose() {
        
    }

    public async Task OpenNomDict(TaskCompletionSource tcs, object? arg = null) {
        await CurrentState.OpenNomDict(tcs, arg);
        tcs.SetResult();
    }
}

public abstract class BaseToolsPanelState : StateBase<ToolsPanel>
{
    public abstract Task OpenNomDict(TaskCompletionSource tcs, object? o);
}

public class ToolsPanelStateIdle : BaseToolsPanelState
{
    protected readonly IServiceProvider _serviceProvider;

    public ToolsPanelStateIdle(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public override async Task OpenNomDict(TaskCompletionSource tcs, object? o) {
        var window = _serviceProvider.GetRequiredService<EntryWindow>();
           
        window.Closed += (_, _) => tcs.SetResult();
        window.Show();
        await tcs.Task;
    }
}