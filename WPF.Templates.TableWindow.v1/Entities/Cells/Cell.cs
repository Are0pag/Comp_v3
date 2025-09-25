using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp.ModelData.TechnicalItems;
using Infrastructure.StateMachine;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Entities.Cells.States;
using WPF.Templates.TableWindow.v1.Events;
using WPF.Templates.TableWindow.v1.Events.Update;

namespace WPF.Templates.TableWindow.v1.Entities.Cells;

public class Cell<TWindow, T> : GenericStateMachine<BaseCellState<TWindow, T>, Cell<TWindow, T>>, ICellEditHandler, IPreviewKeyDownHandler
    where TWindow : Window
    where T : class, IDbEntity
{
    public Cell(IEnumerable<BaseCellState<TWindow, T>> states, BaseCellState<TWindow, T> initialState) : base(states, initialState) {
        EventBus<IGlobSubscriber>.Subscribe(this);
    }

    public virtual void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    public bool IsEnabled { get; protected set; } = true;
    public void SetAccessToHandleCellEvents(bool isEnable) => IsEnabled = isEnable;

    public override Task ChangeState(BaseCellState<TWindow, T> newState, Cell<TWindow, T> context) {
        var changeState = base.ChangeState(newState, context);
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
        Console.WriteLine($"Current Cell state is {newState.GetType().Name}");
        return changeState;
    }

    public override Task RollbackState(BaseCellState<TWindow, T> newState, Cell<TWindow, T> context) {
        var changeState = base.ChangeState(newState, context);
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n.NotifyCanExecute());
        Console.WriteLine($"Current Cell state is {newState.GetType().Name}");
        return changeState;
    }

    public async Task OnEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        if (!IsEnabled) return;
        await CurrentState.OnEnding(this, sender, e);
    }

    public async Task  OnBeginning(object? sender, DataGridBeginningEditEventArgs e) {
        if (!IsEnabled) return;
        await CurrentState.OnBeginning(this, sender, e);
    }

    public async Task OnPreviewKeyDown(object sender, KeyEventArgs e) {
        await CurrentState.OnPreviewKeyDown(this, sender, e);
    }

    public async Task OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        await CurrentState.OnPreviewMouseDown(this, sender, e);
    }
}