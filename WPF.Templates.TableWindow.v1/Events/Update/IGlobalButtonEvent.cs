namespace WPF.Templates.TableWindow.v1.Events.Update;

public interface IGlobalButtonEvent { }

public interface INotifyConditionalsChanged : IGlobalButtonEvent
{
    void NotifyCanExecute();
}