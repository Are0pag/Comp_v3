using System.Windows.Controls;
using System.Windows.Input;
using Infrastructure.Command.Heterochromic;
using Infrastructure.StateMachine;
using WPF.Templates.Core;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.TableWindow.States;

public abstract class BaseState : StateBase<DataGridViewModel>
{
    protected readonly HeterochromicCommandScheduler _scheduler;

    public BaseState(HeterochromicCommandScheduler scheduler) {
        _scheduler = scheduler;
    }

}

public class StateAddItem : BaseState
{
    public StateAddItem(HeterochromicCommandScheduler scheduler) : base(scheduler) {
    }
}

public class StateEditCellText : BaseState
{
    public StateEditCellText(HeterochromicCommandScheduler scheduler) : base(scheduler) {
    }
}