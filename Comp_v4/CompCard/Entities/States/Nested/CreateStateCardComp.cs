using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.NomDict.Events;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Utils.EventBus;

namespace Comp_v4.CompCard.Entities.States;

public class CreateStateCardComp : BaseStateCardComp
{
    public CreateStateCardComp(Component component, IRepository<Component> repository, IRepository<Category> categoryRepository, CardCopmEditController editController) 
        : base(component, repository, categoryRepository, editController) {
    }

    public override async void Save(CardComp card) {
        _editController.ApplyEdits(_component);
        
        _component.Id = default;
        await _repository.AddAsync(_component);
        EventBus<INomDictWindowSubscriber>.RaiseEvent<IComponentUiHandler>(h => h?.OnComponentCardCreated(_component));
    }
}

public class EditStateCardComp : BaseStateCardComp
{
    public EditStateCardComp(Component component, IRepository<Component> repository, IRepository<Category> categoryRepository, CardCopmEditController editController) 
        : base(component, repository, categoryRepository, editController) {
    }
}