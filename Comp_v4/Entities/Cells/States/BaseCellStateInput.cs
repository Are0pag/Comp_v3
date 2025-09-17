using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Transactions;
using Comp.ModelData.TechnicalItems;
using WPF.Extensions.View.Elements;

namespace WPF.Templates.TableWindow.States;

public class BaseCellStateInput : BaseCellState
{
    protected readonly Validator _validator;
    protected BaseAction _action;
    protected DataGridBeginningEditEventArgs? _lastCellEditBeginningEditEventArgs;
    protected RememberCellCommand? _rememberCellCommand;

    public BaseCellStateInput(IModuleCommandScheduler scheduler, ModuleContext context, CommandFactory factory, Validator validator) : base(scheduler, context, factory) {
        _validator = validator;
    }
    
    public override async Task OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        if (!_scheduler.IsInTransaction<TrSelectingCell>())
            return;

        _rememberCellCommand = _commandFactory.CreateCommand<RememberCellCommand, DataGridBeginningEditEventArgs>(e);
        //await _rememberCellCommand.ExecuteAsync();
        await _scheduler.RegisterCommandInto<TrSelectingCell>(_rememberCellCommand)
                        .ExecuteLastRegisteredAsync();
        
        if (_validator.ValidateAsync((ConditionalDesignation)e.Row.Item).Result is { IsValid: true })
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
            case Key.Escape:
            case Key.Tab when _lastCellEditBeginningEditEventArgs!.Column.IsLastVisibleEditableColumn(_context.DataGrid):
                await ContinueWithValidation(async cd => {
                    await _action.PerformAsync(new ActionUpdateItem.Args(_rememberCellCommand!, owner, cd));
                }, owner);
                break;
            
            case Key.Tab:
                await ContinueWithValidation(async cd => {
                    await _action.PerformAsync(new ActionUpdateItem.Args(_rememberCellCommand!, owner, cd));

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
            var prevState = owner.CurrentState;
            await _scheduler.UndoAsync();
            if (prevState is not CellStateAddItem) {
                await owner.CurrentState.OnBeginning(owner, null, _lastCellEditBeginningEditEventArgs);
            }
        }
    }
}