using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Vm.Buttons;
using Utils.WPF;
using WPF.UCL;

namespace Comp_v4.CompCard.Operations.Actions;

public abstract class SetLinkAction : BaseAsyncActionButtonInvoked
{
    protected readonly LinkFieldControlVm _linkFieldVm;
    protected readonly ValidatorUrl _validator;
    public SetLinkAction(LinkFieldControlVm buttonVm, ValidatorUrl validator) : base(buttonVm) {
        _linkFieldVm = buttonVm;
        _validator = validator;
    }

    public override async Task PerformAsync(object? parameter) {
        string desiredUrl = "";
        var window = new OneValueWindow($"{_linkFieldVm.FieldTitle}: ", s => {
            desiredUrl = s;
            return _validator.ValidateAsync(s).Result is { IsValid: true };
        });
        WindowLocator.LocateBy(window).ShowDialog();
        _linkFieldVm.Url = desiredUrl;
    }

    public override bool CanPerform() {
        return true;
    }

    public override async Task CancelAsync(object? parameter = null) {
        throw new NotImplementedException();
    }
}

public class SetUrlAction : SetLinkAction
{
    public SetUrlAction(UrlFieldControlVm buttonVm, ValidatorUrl validator) : base(buttonVm, validator) {
    }
}

public class SetUrlAlternativeAction : SetLinkAction
{
    public SetUrlAlternativeAction(UrlAlternativeFieldControlVm buttonVm, ValidatorUrl validator) : base(buttonVm, validator) {
    }
}

public class SetFilePathAction : SetLinkAction
{
    public SetFilePathAction(FilePathFieldControlVm buttonVm, ValidatorUrl validator) : base(buttonVm, validator) {
    }
}