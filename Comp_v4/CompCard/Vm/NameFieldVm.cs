using Comp_v4.CompCard.Entities.Validation;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Vm;

public class NameFieldVm : BaseRequiredTextFieldVm
{
    public NameFieldVm(ValidatorName validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.Name;
    }
}

public class NomenclatureNumberFieldVm : BaseRequiredTextFieldVm
{
    public NomenclatureNumberFieldVm(ValidatorNomNumber validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.NomenclatureNumber;
    }
}

public class CatalogNumberFieldVm : BaseTextFieldVm
{
    public CatalogNumberFieldVm(ValidatorCatalogNumber validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.CatalogNumber;
    }
}

public class LabelingOptionsFieldVm : BaseTextFieldVm
{
    public LabelingOptionsFieldVm(ValidatorLabelingOptions validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.LabelingOptions;
    }
}

public class CodeOfElementFieldVm : BaseTextFieldVm
{
    public CodeOfElementFieldVm(ValidatorCodeOfElement validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.CodeOfElement;
    }
}

public class gpMainFieldVm : BaseTextFieldVm
{
    public gpMainFieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.GpMain;
    }
}

public class gp1FieldVm : BaseTextFieldVm
{
    public gp1FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.Gp1;
    }
}

public class gp2FieldVm : BaseTextFieldVm
{
    public gp2FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.Gp2;
    }
}

public class gp3FieldVm : BaseTextFieldVm
{
    public gp3FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.Gp3;
    }
}

public class gp4FieldVm : BaseTextFieldVm
{
    public gp4FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.Gp4;
    }
}

public class gp5FieldVm : BaseTextFieldVm
{
    public gp5FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        _value = component.Gp5;
    }
}