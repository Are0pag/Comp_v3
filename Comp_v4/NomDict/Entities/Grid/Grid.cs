using System.Windows.Input;
using Infrastructure.StateMachine;

namespace Comp_v4.NomDict.Entities;

public class Grid : GenericStateMachine<BaseSGridState, Grid>
{
    public Grid(IEnumerable<BaseSGridState> states, BaseSGridState initialState) : base(states, initialState) {
    }

    public virtual void OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        CurrentState.OnMouseDoubleClick(sender, e);
    }

    public virtual void AddComponent(object? parameter) {
        CurrentState.AddComponent(parameter);
    }
}