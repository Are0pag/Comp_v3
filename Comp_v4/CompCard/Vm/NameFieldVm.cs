using Comp_v4.CompCard.Entities.Validation;
using WPF.Services.Validation;
using WPF.Services.ValidationString;

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

public class CatalogNumberFieldVm : BaseTextFieldVm
{
    public CatalogNumberFieldVm(ValidatorCatalogNumber validator) : base(validator) {
    }
}

public class LabelingOptionsFieldVm : BaseTextFieldVm
{
    public LabelingOptionsFieldVm(ValidatorLabelingOptions validator) : base(validator) {
    }
}

public class CodeOfElementFieldVm : BaseTextFieldVm
{
    public CodeOfElementFieldVm(ValidatorCodeOfElement validator) : base(validator) {
    }
}