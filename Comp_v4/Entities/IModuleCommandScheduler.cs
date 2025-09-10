using Infrastructure.Command.Heterochromic;
using Infrastructure.Command.TransactionSupportive;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace Comp_v4.Entities;

public class ModuleCommandScheduler : HeterochromicCommandScheduler<IDeferredCommand, TransactionDeferredSupportive>
{
    public override TransactionalCommandScheduler<IDeferredCommand, TransactionDeferredSupportive> BeginTransaction<TCurrentTransaction>(string? descr = null) {
        return base.BeginTransaction<TCurrentTransaction>(descr);
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
    }
}