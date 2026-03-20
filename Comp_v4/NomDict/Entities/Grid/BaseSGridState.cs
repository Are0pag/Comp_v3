using System.Windows.Input;
using Infrastructure.StateMachine;

namespace Comp_v4.NomDict.Entities;

public abstract class BaseSGridState : StateBase<Grid>
{
    public abstract Task OnMouseDoubleClick(TaskCompletionSource tcs, object sender, MouseButtonEventArgs mouseButtonEventArgs, Grid grid);
    public abstract Task Add(TaskCompletionSource tcs, object? parameter, Grid grid);
    public abstract Task EditComp(TaskCompletionSource tcs, object? parameter, Grid grid);
}

