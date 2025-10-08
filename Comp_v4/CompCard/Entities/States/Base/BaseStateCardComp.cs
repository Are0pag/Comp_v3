using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Events;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.CompCard.Entities.States;

public abstract class BaseStateCardComp : StateBase<CardComp>, IExternalTableInputHandler 
{
    protected readonly Component _component;
    protected readonly IRepository<Component> _repository;
    protected readonly CardCopmEditController _editController;

    protected BaseStateCardComp(Component component, IRepository<Component> repository, CardCopmEditController editController) {
        EventBus<ICompCardSubscriber>.Subscribe(this);
        _component = component;
        _repository = repository;
        _editController = editController;
    }

    public virtual void Save(CardComp card) {
        
    }

    public void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }

    public virtual void HandleTableInput(object? args) {
        if (args is not ConditionalDesignation cd) 
            return;

        _component.ConditionalDesignation = cd;
    }
}