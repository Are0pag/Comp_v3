namespace Comp_v4.Entry.Events;

public interface IEntrySubscriber : IDisposable
{
    
}

public interface IOpenNomDictHandler : IEntrySubscriber
{
    void OpenNomDict(TaskCompletionSource tcs, object? arg = null);
}