namespace Utils.EventBus;

public static class EventBus<TBaseModuleType> 
    where TBaseModuleType : class 
{
    private static readonly Dictionary<Type, SubscribersList<TBaseModuleType>> _subscribers = new();
    private static bool _isExecuting = false;

    public static void Subscribe(TBaseModuleType subscriber) {
        var subscriberTypes = TypeExposer<TBaseModuleType>.GetSubscriberTypes(subscriber);
        foreach (var t in subscriberTypes) {
            if (!_subscribers.ContainsKey(t)) 
                _subscribers[t] = new SubscribersList<TBaseModuleType>();
                
            _subscribers[t].Add(subscriber);
        }
    }

    public static void Unsubscribe(TBaseModuleType subscriber) {
        var subscriberTypes = TypeExposer<TBaseModuleType>.GetSubscriberTypes(subscriber);
        foreach (var t in subscriberTypes) {
            if (_subscribers.TryGetValue(t, out var subscriber1))
                subscriber1.Remove(subscriber);
        }
    }

    public static void RaiseEvent<TSubscriber>(Action<TSubscriber?> action) where TSubscriber : class, TBaseModuleType {
        _isExecuting = true;
        
        // Fixed: Check if subscribers exist before accessing
        if (!_subscribers.TryGetValue(typeof(TSubscriber), out var subscribers)) {
            _isExecuting = false;
            Console.WriteLine("No subscriber found for type {0}", typeof(TSubscriber).Name);
            return;
        }

        subscribers.Executing = true;
        foreach (var subscriber in subscribers.List) {
            try {
                action.Invoke(subscriber as TSubscriber);
            }
            catch (Exception e) {
                // Log the exception but don't crash the application
                System.Diagnostics.Debug.WriteLine($"EventBus error: {e.Message}");
            }
        }

        subscribers.Executing = false;
        subscribers.Cleanup();
        _isExecuting = false;
    }
}