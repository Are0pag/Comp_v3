using Utils.WPF.Buttons;

namespace Utils.WPF;

public abstract class BaseAsyncActionButtonInvoked : BaseAsyncAction
{
    protected readonly BaseAsyncBButtonVm _buttonVm;

    protected BaseAsyncActionButtonInvoked(BaseAsyncBButtonVm buttonVm) {
        _buttonVm = buttonVm;
        buttonVm.CanExecuteFunc = CanPerform;
        buttonVm.ClickActionAsync = PerformAsync;
    }
}