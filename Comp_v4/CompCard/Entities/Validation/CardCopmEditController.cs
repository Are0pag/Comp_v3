using Comp_v4.CompCard.Vm;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Entities.Validation;

public class CardCopmEditController
{
    protected readonly List<BaseTextFieldVm> _fieldViewModels;
    
    public NameFieldVm NameField { get; }
    public NomenclatureNumberFieldVm NomenclatureNumberField { get; }

    public CardCopmEditController(NameFieldVm nameFieldVm, NomenclatureNumberFieldVm nomenclatureNumberFieldVm) {
        _fieldViewModels = new List<BaseTextFieldVm>() {
            nameFieldVm, nomenclatureNumberFieldVm
        };
        
        NameField = nameFieldVm;
        NomenclatureNumberField = nomenclatureNumberFieldVm;
    }

    public bool IsValid() {
        return _fieldViewModels.All(field => field.IsValid());
    }

    public Component ApplyEdits(Component card) {
        card.Name = NameField.Value;
        card.NomenclatureNumber = NomenclatureNumberField.Value;
        return card;
    }
}
