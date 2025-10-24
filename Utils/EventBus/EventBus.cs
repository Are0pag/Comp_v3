namespace Utils.EventBus;

public static class EventBus<TBaseModuleType> 
    where TBaseModuleType : class 
{
    private static readonly Dictionary<Type, SubscribersList<TBaseModuleType>> _subscribers = new();
    private static bool _isExecuting = false;

    public static void Subscribe(TBaseModuleType subscriber) {
        if (_isExecuting)
            throw new InvalidOperationException("Cannot subscribe while executing");
        var subscriberTypes = TypeExposer<TBaseModuleType>.GetSubscriberTypes(subscriber);
        foreach (var t in subscriberTypes) {
            if (!_subscribers.ContainsKey(t)) 
                _subscribers[t] = new SubscribersList<TBaseModuleType>();
                
            _subscribers[t].Add(subscriber);
        }
    }

    public static void Unsubscribe(TBaseModuleType subscriber) {
        if (_isExecuting)
            throw new InvalidOperationException("Cannot subscribe while executing");
        var subscriberTypes = TypeExposer<TBaseModuleType>.GetSubscriberTypes(subscriber);
        foreach (var t in subscriberTypes) {
            if (_subscribers.TryGetValue(t, out var subscriber1))
                subscriber1.Remove(subscriber);
        }
    }

    public static void RaiseEvent<TSubscriber>(Action<TSubscriber?> action) where TSubscriber : class, TBaseModuleType {
        _isExecuting = true;
        var subscribers = _subscribers[typeof(TSubscriber)];

        subscribers.Executing = true;
        foreach (var subscriber in subscribers.List) {
            try {
                action.Invoke(subscriber as TSubscriber);
            }
            catch (Exception e) {
                throw;
            }
        }

        subscribers.Executing = false;
        subscribers.Cleanup();
        _isExecuting = false;
    }
}