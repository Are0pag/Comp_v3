namespace Component_v2.Tools.EventBus;

public static class TypeExposer<TBaseModuleType>
{
    private static readonly Dictionary<Type, List<Type>> _cashedSubscriberTypes = new();
        
    /// <summary>
    /// Find all types, that derived from TBaseModuleType
    /// </summary>
    public static List<Type> GetSubscriberTypes(TBaseModuleType globalSubscriber) {
        var type = globalSubscriber.GetType();
        if (_cashedSubscriberTypes.TryGetValue(type, out var types)) 
            return types;

        var subscriberTypes = type
                             .GetInterfaces()
                             .Where(@interface => @interface.GetInterfaces().Contains(typeof(TBaseModuleType)))
                             .ToList();

        _cashedSubscriberTypes[type] = subscriberTypes;
        return subscriberTypes;
    }
}