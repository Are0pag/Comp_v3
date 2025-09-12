using System.Windows;
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
    protected RememberInputTextCommand? _rememberInputTextCommand;
    protected RememberCellCommand? _rememberCellCommand;
    protected DataGridBeginningEditEventArgs? _lastCellEditBeginningEditEventArgs;

    public CellStateInput(IModuleCommandScheduler scheduler, ModuleContext context, ActionUpdateItem actionUpdateItem) : base(scheduler, context) {
        _actionUpdateItem = actionUpdateItem;
    }

    public override async Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        if (!_scheduler.IsInTransaction<TrSelectingCell>())
            return;
        
        _rememberCellCommand = new RememberCellCommand(e);
        await _rememberCellCommand.ExecuteAsync();
        await _scheduler.RegisterCommandInto<TrSelectingCell>(_rememberCellCommand)
                        .ExecuteLastRegisteredAsync();
        
        _rememberInputTextCommand = new RememberInputTextCommand(e);
        await _rememberInputTextCommand.ExecuteAsync();
        _scheduler.RegisterCommandInto<TrSelectingCell>(_rememberInputTextCommand!);
        _scheduler.CommitTransaction<TrSelectingCell>();
        _lastCellEditBeginningEditEventArgs = e;
    }

    /*public override async Task OnPreviewMouseDown(Cell owner, object sender, MouseButtonEventArgs e) {
        if (_context.DataGrid.IsEditing() && !_context.DataGrid.IsClickInEditingArea(e)) {
            await _actionUpdateItem.CancelAsync();
            await new CellChangeStateCommand(null, owner, owner.GetState<CellStateIdle>()).ExecuteAsync();
        }
    }*/

    /// <summary>
    /// Вызывается до DataGridCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    /// </summary>
    public override async Task OnPreviewKeyDown(Cell owner, object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Enter:
            //case Key.Tab when _context.DataGrid.CurrentColumn.IsLastVisibleEditableColumn(_context.DataGrid):
            case Key.Tab when _lastCellEditBeginningEditEventArgs!.Column.IsLastVisibleEditableColumn(_context.DataGrid):
                await _actionUpdateItem.PerformAsync(new ActionUpdateItem.Args(_rememberCellCommand!, owner));
                break;
            
            case Key.Tab: /* завершается текущая транзакция и явно стимулируется вызов новой */
                await _actionUpdateItem.PerformAsync(new ActionUpdateItem.Args(_rememberCellCommand!, owner));
                
                // Явно переходим к следующей ячейке на основе текущего редактирования
                e.Handled = true;
            
                // Используем Dispatcher для гарантированного выполнения после текущих операций
                await Application.Current.Dispatcher.InvokeAsync(() => {
                    _context.DataGrid.MoveToNextEditableCell(_lastCellEditBeginningEditEventArgs!);
                }, System.Windows.Threading.DispatcherPriority.Input);
                break;
            
            case Key.Escape:
                //await _actionUpdateItem.CancelAsync();
                break;
        }
    }
}