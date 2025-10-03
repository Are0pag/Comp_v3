namespace WPF.Services;

public class TransientRb : IRegistrationBuilder
{
    public TransientRb(RegistrationProxy proxy) {
        Registration = proxy;
    }
    public RegistrationProxy Registration { get; }
    public Func<object>? FactoryResolve { get; set; }

    public virtual object Resolve(AreopagContainer container) {
        if (FactoryResolve != null)
            return FactoryResolve();
        
        /* Код проверяет, что конструктор только один (Length: 1), но не проверяет его доступность.
         Если у класса есть несколько конструкторов (например, private и public), будет выброшено исключение, хотя разрешимый конструктор существует*/
        if (Registration.GetImplementation().GetConstructors() is not { Length: 1 } constructorInfos)
            throw new InvalidOperationException($"Service {Registration.GetImplementation().Name} must have a public constructor");
        
        var parameterInfos = constructorInfos[0].GetParameters();
        
        var parameterInstances = parameterInfos.Select(p => {
            // Проверяем, есть ли runtime-параметр для текущего типа
            if (container.RuntimeParameters.TryGetValue(Registration.GetRegistration(), out var runtimeParams)) {
                try {
                    if (runtimeParams.First(runtimeParam => runtimeParam.Item1.Name == p.ParameterType.Name) is {} runtimeParameter)
                        return runtimeParameter.Item2;
                }
                catch (InvalidOperationException ex) {}
            }
            // Если нет runtime-параметра, резолвим через контейнер
            return container.Resolve(p.ParameterType);
        }).ToArray();
        
        return constructorInfos[0].Invoke(parameterInstances);
    }

    public override string ToString() {
        return Registration.GetImplementation().Name;
    }
}