using System.Windows;
using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Infrastructure.Command;
using WPF.Services.Validation;

namespace WPF.Templates.TableWindow.States;

public class CellStateAddItem<TWindow, T> : BaseCellStateInput<TWindow, T>
    where TWindow : Window
    where T : class
{
    public CellStateAddItem(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context,
                            ICommandFactory factory, ValidatorBase<T> validator, ActionAddItem actionAddItem)
        : base(scheduler, context, factory, validator) {
        _action = actionAddItem;
    }

    public override Task OnBeginning(Cell<TWindow, T> owner, object? sender, DataGridBeginningEditEventArgs e) {
        _rememberCellCommand = _commandFactory.CreateCommand<RememberCellCommand, DataGridBeginningEditEventArgs>(e);
        _lastCellEditBeginningEditEventArgs = e;
        return Task.CompletedTask;
    }
}