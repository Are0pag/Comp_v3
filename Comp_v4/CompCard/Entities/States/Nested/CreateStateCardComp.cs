using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.NomDict.Events;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Utils.EventBus;

namespace Comp_v4.CompCard.Entities.States;

public class CreateStateCardComp : BaseStateCardComp
{
    public CreateStateCardComp(IRepository<Component> repository, CardCopmEditController editController) 
        : base(repository, editController) {
    }

    public override async void Save(CardComp card) {
        _editController.ApplyEdits(RuntimeParam);
        
        RuntimeParam.Id = default;
        await _repository.AddAsync(RuntimeParam);
        EventBus<INomDictWindowSubscriber>.RaiseEvent<IComponentUiHandler>(h => h?.OnComponentCardCreated(RuntimeParam));
    }
}