using Infrastructure.StateMachine;

namespace Comp_v4.CompCard.Entities.States;

public abstract class BaseStateCardComp : StateBase<CardComp>
{
    public virtual void Save(CardComp card) {
        
    }
}