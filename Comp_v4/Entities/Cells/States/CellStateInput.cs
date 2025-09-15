using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using WPF.Extensions.View.Elements;

namespace WPF.Templates.TableWindow.States;

public class CellStateInput : BaseCellState
{
    protected readonly ActionUpdateItem _actionUpdateItem;
    protected readonly Validator _validator;
    protected RememberCellCommand? _rememberCellCommand;
    protected DataGridBeginningEditEventArgs? _lastCellEditBeginningEditEventArgs;

    public CellStateInput(IModuleCommandScheduler scheduler, ModuleContext context, ActionUpdateItem actionUpdateItem, Validator validator) : base(scheduler, context) {
        _actionUpdateItem = actionUpdateItem;
        _validator = validator;
    }

    public override async Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        if (!_scheduler.IsInTransaction<TrSelectingCell>())
            return;
        
        _rememberCellCommand = new RememberCellCommand(e);
        await _rememberCellCommand.ExecuteAsync();
        await _scheduler.RegisterCommandInto<TrSelectingCell>(_rememberCellCommand)
                        .ExecuteLastRegisteredAsync();
        
        await _scheduler.RegisterCommandInto<TrSelectingCell>(new RememberInputTextCommand(e)).ExecuteLastRegisteredAsync();
        _scheduler.CommitTransaction<TrSelectingCell>();
        _lastCellEditBeginningEditEventArgs = e;
    }

    /// <summary>
    /// Вызывается до DataGridCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    /// </summary>
    public override async Task OnPreviewKeyDown(Cell owner, object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Enter:
            case Key.Tab when _lastCellEditBeginningEditEventArgs!.Column.IsLastVisibleEditableColumn(_context.DataGrid):
                await ContinueWithValidation(async cd => {
                    await _actionUpdateItem.PerformAsync(new ActionUpdateItem.Args(_rememberCellCommand!, owner, cd));
                }, owner);
                break;
            
            case Key.Tab:
                await ContinueWithValidation(async cd => {
                    await _actionUpdateItem.PerformAsync(new ActionUpdateItem.Args(_rememberCellCommand!, owner, cd));

                    e.Handled = true; // Используем Dispatcher для гарантированного выполнения после текущих операций
                    await Application.Current.Dispatcher.InvokeAsync(() => {
                        _context.DataGrid.MoveToNextEditableCell(_lastCellEditBeginningEditEventArgs!);
                    }, System.Windows.Threading.DispatcherPriority.Input);
                }, owner);
                break;
        }
    }

    protected virtual async Task ContinueWithValidation(Func<ConditionalDesignation, Task> action, Cell owner) {
        if (_lastCellEditBeginningEditEventArgs!.Row.Item is not ConditionalDesignation item) {
            new Exception().Log(this);
            return;
        }

        if (_validator.ValidateAsync(item).Result is { IsValid: true }) {
            await action(item);
        }
        else {
            await _scheduler.UndoAsync();
            await owner.CurrentState.OnBeginning(owner, null, _lastCellEditBeginningEditEventArgs);
        }
    }
}