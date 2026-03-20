using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Infrastructure.StateMachine;

namespace WPF.Templates.TableWindow.v1.Entities.Cells.States;

public abstract class BaseCellState<TWindow, T> : StateBase<Cell<TWindow, T>>
    where TWindow : Window
    where T : class, IDbEntity
{
    protected readonly IDataGridCommandScheduler _scheduler;
    protected readonly ICommandFactory _commandFactory;
    protected readonly ModuleContext<TWindow, T>  _context;

    public BaseCellState(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory) {
        _scheduler = scheduler;
        _context = context;
        _commandFactory = commandFactory;
    }

    public virtual Task OnEnding(Cell<TWindow, T> owner, object? sender, DataGridCellEditEndingEventArgs e) {
        return Task.CompletedTask;
    }

    public virtual Task  OnBeginning(Cell<TWindow, T> owner, object? sender, DataGridBeginningEditEventArgs e) {
        return Task.CompletedTask;
    }

    public virtual Task  OnPreviewKeyDown(Cell<TWindow, T> owner, object sender, KeyEventArgs e) {
        return Task.CompletedTask;
    }
    
    public virtual Task OnPreviewMouseDown(Cell<TWindow, T> owner, object sender, MouseButtonEventArgs e) {
        return Task.CompletedTask;
    }
}