using Comp_v4.CompCard.Entities.Validation;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Infrastructure.StateMachine;

namespace Comp_v4.CompCard.Entities.States;

public abstract class BaseStateCardComp : StateBase<CardComp>
{
    protected readonly Component _component;
    protected readonly IRepository<Component> _repository;
    protected readonly CardCopmEditController _editController;

    protected BaseStateCardComp(Component component, IRepository<Component> repository, CardCopmEditController editController) {
        _component = component;
        _repository = repository;
        _editController = editController;
    }

    public virtual void Save(CardComp card) {
        
    }
}