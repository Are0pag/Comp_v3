using System.Windows.Input;
using Infrastructure.StateMachine;

namespace Comp_v4.NomDict.Entities;

public abstract class BaseSGridState : StateBase<Grid>
{
    public abstract void OnMouseDoubleClick(object sender, MouseButtonEventArgs e);
    
    public abstract void AddComponent(object? parameter);
}