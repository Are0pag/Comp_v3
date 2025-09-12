using Infrastructure.Command.Heterochromic;
using Infrastructure.Command.TransactionSupportive;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.UCL;

namespace Comp_v4.Entities;

public interface IModuleCommandScheduler : IHeterochromicCommandScheduler<IDeferredCommand, TransactionDeferredSupportive> {}

public class ModuleCommandScheduler : HeterochromicCommandScheduler<IDeferredCommand, TransactionDeferredSupportive>, IModuleCommandScheduler
{
    public override TransactionalCommandScheduler<IDeferredCommand, TransactionDeferredSupportive> BeginTransaction<TCurrentTransaction>(string? descr = null) {
        var r = base.BeginTransaction<TCurrentTransaction>(descr);;
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
        return r;
    }

    public override TransactionalCommandScheduler<IDeferredCommand, TransactionDeferredSupportive> CommitTransaction<TCurrentTransaction>() {
        var r = base.CommitTransaction<TCurrentTransaction>();
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
        return r;
    }

    public override Task<object> UndoAsync() {
        if (!CanUndo())
            NotificationWindow.Show("Uno Stack count = 0");
        return base.UndoAsync();
    }
}