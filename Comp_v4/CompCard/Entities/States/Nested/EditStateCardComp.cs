using Comp_v4.CompCard.Entities.Validation;
using Comp.Db.Contracts;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Entities.States;

public class EditStateCardComp : BaseStateCardComp
{
    public EditStateCardComp(IRepository<Component> repository, CardCopmEditController editController) 
        : base(repository, editController) {
    }

    public override async void Save(CardComp card) {
        _editController.ApplyEdits(RuntimeParam);
        //_component.Id = default;
        await _repository.UpdateAsync(RuntimeParam);
    }
}