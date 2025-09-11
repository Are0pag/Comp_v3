using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using WPF.Extensions.View.Elements;

namespace WPF.Templates.TableWindow.States;

public class CellStateInput : BaseCellState
{
    protected readonly ActionUpdateItem _actionUpdateItem;
    protected bool _isEntry = true;

    public CellStateInput(IModuleCommandScheduler scheduler, ModuleContext context, ActionUpdateItem actionUpdateItem) : base(scheduler, context) {
        _actionUpdateItem = actionUpdateItem;
    }

    public override async Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        if (!_isEntry) 
            return;

        await _actionUpdateItem.PerformAsync(e);
        _isEntry = false;
    }

    public override async Task OnEnding(Cell owner, object? sender, DataGridCellEditEndingEventArgs e) {
        try {
            await _actionUpdateItem.PerformAsync(e);
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    public override async Task OnPreviewMouseDown(Cell owner, object sender, MouseButtonEventArgs e) {
        if (_context.DataGrid.IsEditing() && !_context.DataGrid.IsClickInEditingArea(e)) {
            await _actionUpdateItem.CancelAsync();
            await new CellChangeStateCommand(null, owner, owner.GetState<CellStateIdle>()).ExecuteAsync();
        }
    }

    public override async Task OnPreviewKeyDown(Cell owner, object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Enter:
            case Key.Tab when _context.DataGrid.CurrentColumn.IsLastVisibleEditableColumn(_context.DataGrid):

                
                await _scheduler.RegisterCommandInto<TransactionUpdateItem>(new CellChangeStateCommand(_context, owner, owner.GetState<CellStateIdle>()))
                                .ExecuteLastRegisteredAsync();
                
                _scheduler.CommitTransaction<TransactionUpdateItem>();
                break;
            
            case Key.Tab:
                _scheduler.CommitTransaction<TransactionUpdateItem>();
                break;
            
            case Key.Escape:
                await _actionUpdateItem.CancelAsync();
                break;
        }
    }
    
    /// <summary>
    /// Transaction maybe can started from Idle state
    /// </summary>
    protected void EnsureUpdateTransactionStarted() {
        if (!_scheduler.IsInTransaction<TransactionUpdateItem>()) { 
            _scheduler.BeginTransaction<TransactionUpdateItem>();
        }
    }
}