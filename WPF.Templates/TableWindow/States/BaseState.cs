using Infrastructure.Command.Heterochromic;
using Infrastructure.StateMachine;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.TableWindow.States;

public abstract class BaseState : StateBase<ViewModel>
{
    protected readonly HeterochromicCommandScheduler _scheduler;

    public BaseState(HeterochromicCommandScheduler scheduler) {
        _scheduler = scheduler;
    }
}