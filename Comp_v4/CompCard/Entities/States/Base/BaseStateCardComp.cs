using Comp_v4._Installers;
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
                                          IExternalTableInputHandler<TypeSize>,
                                          IExternalTableInputHandler<GenericParametersSet>,
                                          IRuntimeParamsContainer<Component>
{
    protected readonly IRepository<Component> _repository;
    protected readonly CardCopmEditController _editController;
    protected Component _component;

    protected BaseStateCardComp(IRepository<Component> repository,
                                CardCopmEditController editController) {
        EventBus<ICompCardSubscriber>.Subscribe(this);
        _repository = repository;
        _editController = editController;
    }

    public virtual void Save(CardComp card) { }

    void IExternalTableInputHandler<ConditionalDesignation>.HandleTableInput(ConditionalDesignation? args) => RuntimeParam.ConditionalDesignation = args;
    void IExternalTableInputHandler<Manufacturer>.HandleTableInput(Manufacturer? args) => RuntimeParam.Manufacturer = args;
    void IExternalTableInputHandler<MeasurementUnit>.HandleTableInput(MeasurementUnit? args) => RuntimeParam.MeasurementUnit = args;
    void IExternalTableInputHandler<TypeSize>.HandleTableInput(TypeSize? args) => RuntimeParam.TypeSize = args;
    void IExternalTableInputHandler<GenericParametersSet>.HandleTableInput(GenericParametersSet? args) => RuntimeParam.GenericParametersSet = args;

    public void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }

    public Component RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<Component>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _component;
        }
        set {
            _component = value;
        }
    }
}