using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.Entities;
using Infrastructure.Command.Heterochromic;
using Infrastructure.StateMachine;

namespace WPF.Templates.TableWindow.States;

public abstract class BaseCellState : StateBase<Cell>
{
    protected readonly IModuleCommandScheduler _scheduler;
    protected readonly ModuleContext _context;

    public BaseCellState(IModuleCommandScheduler scheduler, ModuleContext context) {
        _scheduler = scheduler;
        _context = context;
    }

    public virtual async Task OnEnding(Cell owner, object? sender, DataGridCellEditEndingEventArgs e) {
        
    }

    public virtual async Task  OnBeginning(Cell owner, object? sender, DataGridBeginningEditEventArgs e) {
        
    }

    public virtual async Task  OnPreviewKeyDown(Cell owner, object sender, KeyEventArgs e) {
        
    }
}