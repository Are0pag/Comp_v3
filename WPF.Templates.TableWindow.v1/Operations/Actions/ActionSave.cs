using System.Windows;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

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