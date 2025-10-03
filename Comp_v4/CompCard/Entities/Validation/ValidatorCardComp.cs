using Comp_v4.CompCard.Vm;

namespace Comp_v4.CompCard.Entities.Validation;

public class ValidatorCardComp
{
    protected readonly List<BaseTextFieldVm> _fieldViewModels;

    public ValidatorCardComp(NameFieldVm nameFieldVm, NomenclatureNumberFieldVm nomenclatureNumberFieldVm) {
        _fieldViewModels = new List<BaseTextFieldVm>() {
            nameFieldVm, nomenclatureNumberFieldVm
        };
    }

    public bool IsValid() {
        return _fieldViewModels.All(field => field.IsValid());
    }
}