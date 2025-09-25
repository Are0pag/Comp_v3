using System.Windows;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Entities.Cells.States;

namespace WPF.Templates.TableWindow.v1.Operations.Actions;

public class ActionSave<TWindow, T> : BaseAction<TWindow, T> 
    where TWindow : Window
    where T : class, IDbEntity
{
    protected readonly Cell<TWindow, T> _cell;
    public ActionSave(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory, Cell<TWindow, T> cell) 
        : base(scheduler, context, commandFactory) {
        _cell = cell;
    }

    public override async Task<BaseAction<TWindow, T>> PerformAsync(object? parameter = null) {
        try {
            await _scheduler.CommitDeferredChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
        return this;
    }

    public override bool CanPerform() {
        return _scheduler.CanUndo() && _cell.CurrentState is CellStateIdle<TWindow, T>;
    }

    public override Task CancelAsync(object? parameter = null) {
        return Task.CompletedTask;
    }
}