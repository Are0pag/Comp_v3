using System.Windows.Controls;
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

    public override async Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        if (!_scheduler.IsInTransaction<TransactionUpdateItem>()) {
            _scheduler.BeginTransaction<TransactionUpdateItem>();
        }

        await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new RememberCellCommand(_context))
                        .ExecuteLastRegisteredAsync();
        
        await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new RememberInputTextCommand(_context))
                        .ExecuteLastRegisteredAsync();
    }

    public override async Task OnPreviewKeyDown(Cell owner, object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Enter:
            case Key.Tab when _context.DataGrid.CurrentColumn.IsLastVisibleEditableColumn(_context.DataGrid):
                try {
                    await _actionUpdateItem.PerformAsync();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
                
                await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new CellChangeStateCommand(_context, owner, owner.GetState<CellStateIdle>()))
                                .ExecuteLastRegisteredAsync();
                
                _scheduler.CommitTransaction<TransactionUpdateItem>();
                break;
            
            case Key.Tab:
                await _actionUpdateItem.PerformAsync();
                _scheduler.CommitTransaction<TransactionUpdateItem>();
                break;
            
            case Key.Escape:
                await _actionUpdateItem.CancelAsync();
                break;
        }
    }
}