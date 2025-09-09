using System.Windows.Controls;
using System.Windows.Input;
using Infrastructure.Command.Heterochromic;
using Infrastructure.StateMachine;

namespace WPF.Templates.TableWindow.States;

public abstract class BaseCellState : StateBase<ModuleContext>
{
    protected readonly HeterochromicCommandScheduler _scheduler;
    protected readonly ModuleContext _context;

    public BaseCellState(HeterochromicCommandScheduler scheduler, ModuleContext context) {
        _scheduler = scheduler;
        _context = context;
    }

    public virtual void OnEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        
    }

    public virtual void OnBeginning(object? sender, DataGridBeginningEditEventArgs e) {
        
    }

    public virtual void OnPreviewKeyDown(object sender, KeyEventArgs e) {
        
    }
}