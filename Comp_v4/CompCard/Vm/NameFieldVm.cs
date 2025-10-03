using Comp_v4.CompCard.Entities.Validation;
using WPF.Services.Validation;

namespace Comp_v4.CompCard.Vm;

public class NameFieldVm : BaseTextFieldVm
{
    public NameFieldVm(ValidatorName validator) : base(validator) {
    }
}

public class NomenclatureNumberFieldVm : BaseTextFieldVm
{
    public NomenclatureNumberFieldVm(ValidatorNomNumber validator) : base(validator) {
    }
}