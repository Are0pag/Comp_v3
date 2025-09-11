using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace WPF.Templates;

public class ActionUpdateItem : BaseAction
{
    public ActionUpdateItem(IModuleCommandScheduler scheduler, ModuleContext context) : base(scheduler, context) {
    }

    public async Task PerformOnFirstEditAsync(DataGridBeginningEditEventArgs e) {
        await Begin(e);
        _scheduler.CommitTransaction<TransactionUpdateItem>();
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        if (parameter is DataGridBeginningEditEventArgs e)
            await Begin(e);

        return this;
    }

    private async Task Begin(DataGridBeginningEditEventArgs e) {
        _scheduler.BeginTransaction<TransactionUpdateItem>();
        
        EventBus<IGlobSubscriber>.RaiseEvent<ICellEditHandler>(h => h.SetAccessToHandleCellEvents(false));
        
        await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new RememberCellCommand(_context, e))
                        .ExecuteLastRegisteredAsync();
        
        await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new RememberInputTextCommand(_context, e.Row))
                        .ExecuteLastRegisteredAsync();
        
        EventBus<IGlobSubscriber>.RaiseEvent<ICellEditHandler>(h => h.SetAccessToHandleCellEvents(true));
        
        await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new UpdateItemCommand(_context))
                        .ExecuteLastRegisteredAsync();
    }

    public override bool CanPerform() {
        return true;
    }

    public override async Task CancelAsync(object? parameter = null) {
        try {
            _scheduler.CommitTransaction<TransactionUpdateItem>();
        }
        catch (Exception e) {
            Console.WriteLine(e);
        } 
    }
}