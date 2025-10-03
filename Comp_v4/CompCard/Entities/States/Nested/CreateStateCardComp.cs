using Comp.Db.Contracts;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Entities.States;

public class CreateStateCardComp : BaseStateCardComp
{
    public CreateStateCardComp(Component component, IRepository<Component> repository) : base(component, repository) {
    }

    public override void Save(CardComp card) {
        /*var dbCategory = _repository.GetByIdAsync(_component.Category.Id).Result;
        _component.Category = dbCategory;*/
        
        
        _component.Id = default;
        _repository.AddAsync(_component);
    }
}

public class EditStateCardComp : BaseStateCardComp
{
    public EditStateCardComp(Component component, IRepository<Component> repository) : base(component, repository) {
    }
}