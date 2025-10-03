using Comp_v4.CompCard.Vm;

namespace Comp_v4.CompCard.Entities.Validation;

public class ValidatorCardComp
{
    protected readonly List<BaseFieldVm> _fieldViewModels;

    public ValidatorCardComp(NameFieldVm nameFieldVm, NomenclatureNumberFieldVm nomenclatureNumberFieldVm) {
        _fieldViewModels = new List<BaseFieldVm>() {
            nameFieldVm, nomenclatureNumberFieldVm
        };
    }

    public bool IsValid() {
        return _fieldViewModels.All(field => field.IsValid());
    }
}