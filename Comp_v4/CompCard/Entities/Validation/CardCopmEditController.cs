using Comp_v4.CompCard.Vm;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Entities.Validation;

public class CardCopmEditController
{
    protected readonly List<BaseTextFieldVm> _fieldViewModels;
    
    // Changed to public with a get-only property
    public NameFieldVm NameField { get; }
    public NomenclatureNumberFieldVm NomenclatureNumberField { get; }

    public CardCopmEditController(NameFieldVm nameFieldVm, NomenclatureNumberFieldVm nomenclatureNumberFieldVm) {
        _fieldViewModels = new List<BaseTextFieldVm>() {
            nameFieldVm, nomenclatureNumberFieldVm
        };
        
        // Assign to the new public properties
        NameField = nameFieldVm;
        NomenclatureNumberField = nomenclatureNumberFieldVm;
    }

    public bool IsValid() {
        return _fieldViewModels.All(field => field.IsValid());
    }

    public Component ApplyEdits(Component card) {
        // Update to use the new public properties
        card.Name = NameField.Value;
        card.NomenclatureNumber = NomenclatureNumberField.Value;
        return card;
    }
}
