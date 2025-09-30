namespace Utils.WPF.Buttons;

public interface IGlobalButtonEvent : IDisposable { }

public interface INotifyConditionalsChanged : IGlobalButtonEvent
{
    void NotifyCanExecute();
}