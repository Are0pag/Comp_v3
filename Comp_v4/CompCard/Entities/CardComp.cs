using Comp_v4.CompCard.Entities.States;
using Infrastructure.StateMachine;
using WPF.Services;

namespace Comp_v4.CompCard.Entities;

public class CardComp : GenericStateMachine<BaseStateCardComp, CardComp>
{
    protected readonly AreopagContainer _container;
    protected CardComp(IEnumerable<BaseStateCardComp> states, BaseStateCardComp initialState, AreopagContainer container) : base(states, initialState) {
        _container = container;
    }

    public void Save() {
        CurrentState.Save(this);
    }
    
    public void OpenWindow(BaseStateCardComp stateOnOpen) {
        var window = _container.BeginScope<CompCardWindow>();
        window.Closed += (_, __) => _container.ReleaseScope<CompCardWindow>();
        CurrentState = stateOnOpen;
        window.Show();
    }
}
