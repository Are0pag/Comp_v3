using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.States;

public abstract class BaseStateGridHandler : BaseState
{
    protected BaseStateGridHandler(HeterochromicCommandScheduler scheduler) : base(scheduler) {
    }
}