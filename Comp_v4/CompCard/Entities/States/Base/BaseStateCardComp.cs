using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Infrastructure.StateMachine;

namespace Comp_v4.CompCard.Entities.States;

public abstract class BaseStateCardComp : StateBase<CardComp>
{
    protected readonly Component _component;
    protected readonly IRepository<Component> _repository;
    protected readonly IRepository<Category> _categoryRepository;

    protected BaseStateCardComp(Component component, IRepository<Component> repository, IRepository<Category> categoryRepository) {
        _component = component;
        this._repository = repository;
        _categoryRepository = categoryRepository;
    }

    public virtual void Save(CardComp card) {
        
    }
}