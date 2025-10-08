using Comp_v4.CompCard.Entities.Validation;
using WPF.Services.Validation;
using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Vm;

public class NameFieldVm : BaseRequiredTextFieldVm
{
    public NameFieldVm(ValidatorName validator) : base(validator) {
    }
}

public class NomenclatureNumberFieldVm : BaseRequiredTextFieldVm
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

public class gpMainFieldVm : BaseTextFieldVm
{
    public gpMainFieldVm(ValidatorGp validator) : base(validator) {
    }
}

public class gp1FieldVm : BaseTextFieldVm
{
    public gp1FieldVm(ValidatorGp validator) : base(validator) {
    }
}

public class gp2FieldVm : BaseTextFieldVm
{
    public gp2FieldVm(ValidatorGp validator) : base(validator) {
    }
}

public class gp3FieldVm : BaseTextFieldVm
{
    public gp3FieldVm(ValidatorGp validator) : base(validator) {
    }
}

public class gp4FieldVm : BaseTextFieldVm
{
    public gp4FieldVm(ValidatorGp validator) : base(validator) {
    }
}

public class gp5FieldVm : BaseTextFieldVm
{
    public gp5FieldVm(ValidatorGp validator) : base(validator) {
    }
}