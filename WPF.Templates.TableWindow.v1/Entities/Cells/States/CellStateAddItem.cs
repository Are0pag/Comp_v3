using System.Windows;
using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Services.Validation;

namespace WPF.Templates.TableWindow.States;

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
        _rememberCellCommand = _commandFactory.CreateCommand<RememberCellCommand<TWindow, T>, DataGridBeginningEditEventArgs>(e);
        _lastCellEditBeginningEditEventArgs = e;
        return Task.CompletedTask;
    }
}