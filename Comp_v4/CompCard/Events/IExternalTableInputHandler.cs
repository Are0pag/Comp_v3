namespace Comp_v4.CompCard.Events;

public interface IExternalTableInputHandler : ICompCardSubscriber
{
    void HandleTableInput(object? args);
}