using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using Infrastructure.Command;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.Validation;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Operations.Commands.Ui;
using WPF.Templates.TableWindow.v1.Operations.Transactions;

namespace WPF.Templates.TableWindow.v1.Entities.Cells.States;

public class BaseCellStateInput<TWindow, T> : BaseCellState<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity
{
    protected readonly ValidatorBase<T> _validator;
    protected readonly IPropertyValueRestoreService<T> _propertyRestoreService;
    protected BaseAction<TWindow, T> _action;
    protected DataGridBeginningEditEventArgs? _lastCellEditBeginningEditEventArgs;
    protected RememberCellCommand<TWindow, T>? _rememberCellCommand;
    protected bool _isPreviewKeyDownHandled = false;

    public BaseCellStateInput(IDataGridCommandScheduler scheduler, 
                              ModuleContext<TWindow, T> context, 
                              ICommandFactory factory, 
                              ValidatorBase<T> validator, 
                              IPropertyValueRestoreService<T> propertyRestoreService) 
        : base(scheduler, context, factory) {
        _validator = validator;
        _propertyRestoreService = propertyRestoreService;
    }
    
    public override async Task OnBeginning(Cell<TWindow, T> owner, object? sender, DataGridBeginningEditEventArgs e) {
        if (!_scheduler.IsInTransaction<TrSelectingCell>())
            return;

        _rememberCellCommand = new RememberCellCommand<TWindow, T>(new RememberCellCommand<TWindow, T>.Args(e, Application.Current.Dispatcher), _context);
        
        await _scheduler.RegisterCommandInto<TrSelectingCell>(_rememberCellCommand)
                        .ExecuteLastRegisteredAsync();

        if (_validator.ValidateAsync((T)e.Row.Item).Result is { IsValid: true }) {
            var command = new RememberInputTextCommand<TWindow, T>(e, _propertyRestoreService);
            await _scheduler.RegisterCommandInto<TrSelectingCell>(command)
                            .ExecuteLastRegisteredAsync();
        }
        
        _scheduler.CommitTransaction<TrSelectingCell>();
        _lastCellEditBeginningEditEventArgs = e;
        _isPreviewKeyDownHandled = false;
    }
    
    public override async Task OnPreviewMouseDown(Cell<TWindow, T> owner, object sender, MouseButtonEventArgs e) {
        if (_context.DataGrid.IsEditing() && !_context.DataGrid.IsClickInEditingArea(e)) {
            await _action.CancelAsync();
        }
    }

    /// <summary>
    /// Вызывается до DataGridCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    /// </summary>
    public override async Task OnPreviewKeyDown(Cell<TWindow, T> owner, object sender, KeyEventArgs e) {
        if (_isPreviewKeyDownHandled)
            return;
        switch (e.Key) {
            case Key.Enter:
            //case Key.Return: // Return - старое название, появившееся еще в машинописных клавиатурах, где клавиша возвращала каретку на начало строки
            case Key.Escape:
            case Key.Tab when _lastCellEditBeginningEditEventArgs!.Column.IsLastVisibleEditableColumn(_context.DataGrid):
                await ContinueWithValidation(async cd => {
                    try {
                        await _action.PerformAsync(new ActionUpdateItem<TWindow, T>.Args(_rememberCellCommand!, owner, cd));
                    }
                    catch (Exception exception) {
                        throw new Exception(exception.Message);
                    }
                }, owner);
                _isPreviewKeyDownHandled = true;
                break;
            
            case Key.Tab:
                await ContinueWithValidation(async cd => {
                    await _action.PerformAsync(new ActionUpdateItem<TWindow, T>.Args(_rememberCellCommand!, owner, cd));

                    e.Handled = true; // Используем Dispatcher для гарантированного выполнения после текущих операций
                    await Application.Current.Dispatcher.InvokeAsync(() => {
                        _context.DataGrid.MoveToNextEditableCell(_lastCellEditBeginningEditEventArgs!);
                    }, System.Windows.Threading.DispatcherPriority.Input);
                }, owner);
                _isPreviewKeyDownHandled = true;
                break;
        }
    }

    protected virtual async Task ContinueWithValidation(Func<T, Task> action, Cell<TWindow, T> owner) {
        if (_lastCellEditBeginningEditEventArgs.Row.Item is not T item) {
            new Exception().Log(this);
            return;
        }

        if (_validator.ValidateAsync(item).Result is { IsValid: true }) {
        #if DEBUG
            Console.WriteLine($"valid: true");
        #endif
            await action(item);
        }
        else {
        #if DEBUG
            Console.WriteLine($"valid: false");
        #endif
            var prevState = owner.CurrentState;
            await _scheduler.UndoAsync();
            if (prevState is not CellStateAddItem<TWindow, T>) {
                await owner.CurrentState.OnBeginning(owner, null, _lastCellEditBeginningEditEventArgs);
            }
        }
    }
}