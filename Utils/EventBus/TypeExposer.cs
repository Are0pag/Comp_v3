namespace Utils.EventBus;

public static class TypeExposer<TBaseModuleType>
{
    private static readonly Dictionary<Type, List<Type>> _cashedSubscriberTypes = new();
        
    /// <summary>
    /// Find all types, that derived from TBaseModuleType
    /// </summary>
    /// <param name="globalSubscriber"> Take an instance of the type, that implements one or more interfaces, derived from TBaseModuleType </param>
    /// <returns> Return types of implemented intrfaces </returns>
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