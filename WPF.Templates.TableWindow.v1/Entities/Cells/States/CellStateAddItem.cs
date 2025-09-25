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
    public CellStateAddItem(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context,
                            ICommandFactory factory, ValidatorBase<T> validator, ActionAddItem<TWindow, T> actionAddItem)
        : base(scheduler, context, factory, validator) {
        _action = actionAddItem;
    }

    public override Task OnBeginning(Cell<TWindow, T> owner, object? sender, DataGridBeginningEditEventArgs e) {
        _rememberCellCommand = _commandFactory.CreateCommand<
            RememberCellCommand<TWindow, T>, 
            RememberCellCommand<TWindow, T>.Args>(new RememberCellCommand<TWindow, T>.Args(
                                                      e, Application.Current.Dispatcher));
        _lastCellEditBeginningEditEventArgs = e;
        return Task.CompletedTask;
    }
}