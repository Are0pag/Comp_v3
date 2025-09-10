using System.Windows.Input;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using WPF.Extensions.View.Elements;

namespace WPF.Templates.TableWindow.States;

public class CellStateInput : BaseCellState
{
    protected readonly ActionUpdateItem _actionUpdateItem;
    public CellStateInput(IModuleCommandScheduler scheduler, ModuleContext context, ActionUpdateItem actionUpdateItem) : base(scheduler, context) {
        _actionUpdateItem = actionUpdateItem;
    }

    public override async Task OnPreviewKeyDown(Cell owner, object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Enter:
            case Key.Tab when _context.DataGrid.CurrentColumn.IsLastVisibleEditableColumn(_context.DataGrid):
                await _actionUpdateItem.PerformAsync();
                await _scheduler.ExecuteCommand(new CellChangeStateCommand(_context, owner, owner.GetState<CellStateIdle>()));
                break;
            
            case Key.Tab:
                await _actionUpdateItem.PerformAsync();
                break;
            
            case Key.Escape:
                await _actionUpdateItem.CancelAsync();
                break;
        }
    }
    
    protected virtual async Task CreateUpdateItemTransaction() {
        await _scheduler.BeginTransaction<TransactionUpdateItem>()
                        .RegisterCommand(new RememberInputTextCommand(_context))
                        .ExecuteLastRegisteredAsync();
                
        await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new UpdateItemCommand(_context))
                        .ExecuteLastRegisteredAsync();

        _scheduler.CommitTransaction<TransactionUpdateItem>();
    }
}