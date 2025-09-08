using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.States;

public abstract class BaseStateHotKey : BaseState
{
    protected BaseStateHotKey(HeterochromicCommandScheduler scheduler) : base(scheduler) {
    }
}