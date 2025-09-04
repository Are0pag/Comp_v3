namespace Comp_v3.Front.Events.Buttons;

public interface IGlobalButtonEvent
{
    
}

public interface INotifyConditionalsChanged : IGlobalButtonEvent
{
    void NotifyCanExecute();
}