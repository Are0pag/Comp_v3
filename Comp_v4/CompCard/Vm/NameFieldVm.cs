using Comp_v4.CompCard.Entities.Validation;
using WPF.Services.Validation;

namespace Comp_v4.CompCard.Vm;

public class NameFieldVm : BaseFieldVm
{
    public NameFieldVm(ValidatorName validator) : base(validator) {
    }
}

public class NomenclatureNumberFieldVm : BaseFieldVm
{
    public NomenclatureNumberFieldVm(ValidatorNomNumber validator) : base(validator) {
    }
}