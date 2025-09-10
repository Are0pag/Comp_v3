using System.Windows.Controls;
using System.Windows.Input;
using Infrastructure.StateMachine;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.States;

namespace WPF.Templates;

public class Cell : GenericStateMachine<BaseCellState, ModuleContext>, ICellEditHandler, IPreviewKeyDownHandler
{
    public Cell(IEnumerable<BaseCellState> states, BaseCellState initialState) : base(states, initialState) {
        EventBus<IGlobSubscriber>.Subscribe(this);
    }

    public virtual void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    public bool IsEnabled { get; set; } = true;

    public async Task OnEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        if (!IsEnabled) return;
        await CurrentState.OnEnding(sender, e);
    }

    public async Task  OnBeginning(object? sender, DataGridBeginningEditEventArgs e) {
        if (!IsEnabled) return;
        await CurrentState.OnBeginning(sender, e);
    }

    public async Task  OnPreviewKeyDown(object sender, KeyEventArgs e) {
        await CurrentState.OnPreviewKeyDown(sender, e);
    }
}