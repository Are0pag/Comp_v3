using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Infrastructure.StateMachine;

namespace Comp_v4.CompCard.Entities.States;

public abstract class BaseStateCardComp : StateBase<CardComp>
{
    protected readonly Component _component;
    protected readonly IRepository<Component> _repository;

    protected BaseStateCardComp(Component component, IRepository<Component> repository) {
        _component = component;
        this._repository = repository;
    }

    public virtual void Save(CardComp card) {
        
    }
}