namespace WPF.Services;

public class TransientRb : IRegistrationBuilder
{
    public TransientRb(RegistrationProxy proxy) {
        Registration = proxy;
    }
    public RegistrationProxy Registration { get; }

    public virtual object Resolve(Container container) {
        
        /* Код проверяет, что конструктор только один (Length: 1), но не проверяет его доступность.
         Если у класса есть несколько конструкторов (например, private и public), будет выброшено исключение, хотя разрешимый конструктор существует*/
        if (Registration.GetImplementation().GetConstructors() is not { Length: 1 } constructorInfos)
            throw new InvalidOperationException($"Service {Registration.GetImplementation().Name} must have a public constructor");
        
        var parameterInfos = constructorInfos[0].GetParameters();
        var parameterInstances = parameterInfos.Select(p => {
            // Проверяем, есть ли runtime-параметр для текущего типа
            if (container.RuntimeParameters.TryGetValue(Registration.GetRegistration(), out var runtimeParams)) {
                return runtimeParams.FirstOrDefault(runtimeParam => runtimeParam.Item1 == p.ParameterType).Item2;
            }
            // Если нет runtime-параметра, резолвим через контейнер
            return container.Resolve(p.ParameterType);
        }).ToArray();
        
        return constructorInfos[0].Invoke(parameterInstances);
    }
}

