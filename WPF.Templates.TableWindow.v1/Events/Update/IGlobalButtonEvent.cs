namespace WPF.Templates.TableWindow.Events;

public interface IGlobalButtonEvent { }

public interface INotifyConditionalsChanged : IGlobalButtonEvent
{
    void NotifyCanExecute();
}