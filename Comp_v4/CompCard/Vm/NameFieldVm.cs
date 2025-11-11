using Comp_v4.CompCard.Entities.Validation;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;

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

public class QrCodeDataFieldVm : BaseTextFieldVm
{
    public QrCodeDataFieldVm(ValidatorQrCodeData validator) : base(validator) {
        Label = "Данные для QR-кода: ";
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.QrCodeData;
    }
}

public class DescriptionFieldVm : BaseTextFieldVm
{
    public DescriptionFieldVm(ValidatorDescription validator) : base(validator) {
        Label = "Описание компонента: ";
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.Description;
    }
}

public class CommentsFieldVm : BaseTextFieldVm
{
    public CommentsFieldVm(ValidatorComments validator) : base(validator) {
        Label = "Комментарии: ";
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.Comments;
    }
}

public class gpMainFieldVm : BaseGpsTextFieldVm
{
    public gpMainFieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.GpMain;
        Label = component.GenericParametersSet?.GpMain ?? DEFAULT_GPS_KEY;
    }

    public override void HandleTableInput(GenericParametersSet? args) {
        Label = args?.GpMain ?? DEFAULT_GPS_KEY;
    }
}

public class gp1FieldVm : BaseGpsTextFieldVm
{
    public gp1FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.Gp1;
        Label = component.GenericParametersSet?.Gp1 ?? DEFAULT_GPS_KEY;
    }

    public override void HandleTableInput(GenericParametersSet? args) {
        Label = args?.Gp1 ?? DEFAULT_GPS_KEY;
    }
}

public class gp2FieldVm : BaseGpsTextFieldVm
{
    public gp2FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.Gp2;
        Label = component.GenericParametersSet?.Gp2 ?? DEFAULT_GPS_KEY;
    }

    public override void HandleTableInput(GenericParametersSet? args) {
        Label = args?.Gp2 ?? DEFAULT_GPS_KEY;
    }
}

public class gp3FieldVm : BaseGpsTextFieldVm
{
    public gp3FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.Gp3;
        Label = component.GenericParametersSet?.Gp3 ?? DEFAULT_GPS_KEY;
    }

    public override void HandleTableInput(GenericParametersSet? args) {
        Label = args?.Gp3 ?? DEFAULT_GPS_KEY;
    }
}

public class gp4FieldVm : BaseGpsTextFieldVm
{
    public gp4FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.Gp4;
        Label = component.GenericParametersSet?.Gp4 ?? DEFAULT_GPS_KEY;
    }

    public override void HandleTableInput(GenericParametersSet? args) {
        Label = args?.Gp4 ?? DEFAULT_GPS_KEY;
    }
}

public class gp5FieldVm : BaseGpsTextFieldVm
{
    public gp5FieldVm(ValidatorGp validator) : base(validator) {
    }

    public override void OnCompCardLoaded(Component component) {
        Value = component.Gp5;
        Label = component.GenericParametersSet?.Gp5 ?? DEFAULT_GPS_KEY;
    }

    public override void HandleTableInput(GenericParametersSet? args) {
        Label = args?.Gp5 ?? DEFAULT_GPS_KEY;
    }
}