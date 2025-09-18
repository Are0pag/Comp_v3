using System.Windows;
using Infrastructure.Command.Heterochromic;
using Infrastructure.Command.TransactionSupportive;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.UCL;

namespace Comp_v4.Entities;

public interface IDataGridCommandScheduler : IHeterochromicCommandScheduler<IDeferredCommand, TransactionDeferredSupportive> {}

public class DataGridCommandScheduler : HeterochromicCommandScheduler<IDeferredCommand, TransactionDeferredSupportive>, IDataGridCommandScheduler
{
    public override TransactionalCommandScheduler<IDeferredCommand, TransactionDeferredSupportive> BeginTransaction<TCurrentTransaction>(string? descr = null) {
        var r = base.BeginTransaction<TCurrentTransaction>(descr);;
        Notify();
        return r;
    }

    public override TransactionalCommandScheduler<IDeferredCommand, TransactionDeferredSupportive> CommitTransaction<TCurrentTransaction>() {
        var r = base.CommitTransaction<TCurrentTransaction>();
        Notify();
        return r;
    }

    public override Task<object> UndoAsync() {
        if (!CanUndo()) {
            // Запускаем в отдельном потоке чтобы не блокировать UI
            Task.Run(() => {
                Application.Current.Dispatcher.Invoke(() => {
                    var notification = new NotificationWindow("Undo Stack = 0");
                    notification.Show();
                });
            });
        }
        Notify();
        return base.UndoAsync();
    }

    protected static void Notify() {
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n?.NotifyCanExecute());
    }
}