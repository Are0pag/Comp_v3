
namespace Comp_v4.CompCard.Vm;

public abstract class BaseVmForFieldWithButton : BaseFieldVm
{
    protected readonly Action _openWindow;

    protected BaseVmForFieldWithButton(Action openWindow) {
        _openWindow = openWindow;
    }

    protected abstract void OpenNestedWindow();
}