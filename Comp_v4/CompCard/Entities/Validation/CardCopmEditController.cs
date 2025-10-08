using Comp_v4.CompCard.Vm;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Entities.Validation;

public class CardCopmEditController
{
    protected readonly List<BaseTextFieldVm> _requaredFieldViewModels;
    
    public NameFieldVm NameField { get; }
    public NomenclatureNumberFieldVm NomenclatureNumberField { get; }
    
    public CatalogNumberFieldVm CatalogNumberField { get; }
    public LabelingOptionsFieldVm LabelingOptionsField { get; }

    public CardCopmEditController(NameFieldVm nameFieldVm, NomenclatureNumberFieldVm nomenclatureNumberFieldVm, CatalogNumberFieldVm catalogNumberField, LabelingOptionsFieldVm labelingOptionsField) {
        _requaredFieldViewModels = new List<BaseTextFieldVm>() {
            nameFieldVm, nomenclatureNumberFieldVm
        };
        
        NameField = nameFieldVm;
        NomenclatureNumberField = nomenclatureNumberFieldVm;
        CatalogNumberField = catalogNumberField;
        LabelingOptionsField = labelingOptionsField;
    }

    public bool IsValid() {
        return _requaredFieldViewModels.All(field => field.IsValid());
    }

    public Component ApplyEdits(Component card) {
        card.Name = NameField.Value;
        card.NomenclatureNumber = NomenclatureNumberField.Value;
        
        if (CatalogNumberField.IsValid())
            card.CatalogNumber = CatalogNumberField.Value;
        if (LabelingOptionsField.IsValid())
            card.LabelingOptions = LabelingOptionsField.Value;
        return card;
    }
}
