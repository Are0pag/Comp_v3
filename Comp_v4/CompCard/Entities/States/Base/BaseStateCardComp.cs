using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Events;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;
using Infrastructure.StateMachine;
using Utils.EventBus;

namespace Comp_v4.CompCard.Entities.States;

public abstract class BaseStateCardComp : StateBase<CardComp>,
                                          IExternalTableInputHandler<ConditionalDesignation>,
                                          IExternalTableInputHandler<Manufacturer>,
                                          IExternalTableInputHandler<MeasurementUnit>,
                                          IExternalTableInputHandler<TypeSize>
{
    protected readonly Component _component;
    protected readonly IRepository<Component> _repository;
    protected readonly CardCopmEditController _editController;

    protected BaseStateCardComp(Component component, IRepository<Component> repository,
                                CardCopmEditController editController) {
        EventBus<ICompCardSubscriber>.Subscribe(this);
        _component = component;
        _repository = repository;
        _editController = editController;
    }

    public virtual void Save(CardComp card) { }

    void IExternalTableInputHandler<ConditionalDesignation>.HandleTableInput(ConditionalDesignation? args) => _component.ConditionalDesignation = args;
    void IExternalTableInputHandler<Manufacturer>.HandleTableInput(Manufacturer? args) => _component.Manufacturer = args;
    void IExternalTableInputHandler<MeasurementUnit>.HandleTableInput(MeasurementUnit? args) => _component.MeasurementUnit = args;
    void IExternalTableInputHandler<TypeSize>.HandleTableInput(TypeSize? args) => _component.TypeSize = args;

    public void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }
}