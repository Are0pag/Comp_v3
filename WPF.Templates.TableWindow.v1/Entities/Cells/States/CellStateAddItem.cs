using System.Windows;
using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Services.Validation;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Operations.Commands.Ui;

namespace WPF.Templates.TableWindow.v1.Entities.Cells.States;

public class CellStateAddItem<TWindow, T> : BaseCellStateInput<TWindow, T>
    where TWindow : Window
    where T : class, IDbEntity
{
    protected bool _isBeginningHandled = false;
    public CellStateAddItem(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context,
                            ICommandFactory factory, ValidatorBase<T> validator, ActionAddItem<TWindow, T> actionAddItem)
        : base(scheduler, context, factory, validator) {
        _action = actionAddItem;
    }

    public override Task OnBeginning(Cell<TWindow, T> owner, object? sender, DataGridBeginningEditEventArgs e) {
        if (_isBeginningHandled)
            return Task.CompletedTask;
        
        _rememberCellCommand = _commandFactory.CreateCommand<
            RememberCellCommand<TWindow, T>, 
            RememberCellCommand<TWindow, T>.Args>(new RememberCellCommand<TWindow, T>.Args(
                                                      e, Application.Current.Dispatcher));
        _lastCellEditBeginningEditEventArgs = e;
        _isBeginningHandled = true;
        
    #if DEBUG
        Console.WriteLine("Edit Beginning Details:");
        Console.WriteLine($"  {"Row Index",-20}: {e.Row.GetIndex()}");
        Console.WriteLine($"  {"Column Index",-20}: {e.Column.DisplayIndex}");
        Console.WriteLine($"  {"Column Header",-20}: {e.Column.Header}");
        Console.WriteLine($"  {"Row Item Type",-20}: {e.Row.Item?.GetType().Name ?? "null"}");
        Console.WriteLine($"  {"Row Item Details",-20}: {e.Row.Item}");
        Console.WriteLine($"  {"Is Begin Edit",-20}: {e.Cancel}");
    #endif
        return Task.CompletedTask;
    }
}