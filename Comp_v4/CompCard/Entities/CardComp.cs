using Comp_v4.CompCard.Entities.States;
using Infrastructure.StateMachine;

namespace Comp_v4.CompCard.Entities;

public class CardComp : GenericStateMachine<BaseStateCardComp, CardComp>
{
    public CardComp(IEnumerable<BaseStateCardComp> states, BaseStateCardComp initialState) : base(states, initialState) {
        
    }

    public void Save() {
        CurrentState.Save(this);
    }
}
