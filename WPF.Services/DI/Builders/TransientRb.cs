using System.Reflection;
using Infrastructure;

namespace WPF.Services;

public class TransientRb : IRegistrationBuilder
{
    public TransientRb(RegistrationProxy proxy) {
        Registration = proxy;
    }
    public RegistrationProxy Registration { get; protected set; }
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
            object parameter;
            try {
                parameter = container.Resolve(p.ParameterType);
            }
            catch (Exception ex) {
                throw new ApplicationException($"Could not resolve parameter {p.ParameterType} when resolving {Registration.GetImplementation().Name} because {ex.Message}");
            }
            return parameter;
        }).ToArray();
        
        return constructorInfos[0].Invoke(parameterInstances);
    }

    public void OverrideImplementation(RegistrationProxy newImplementation) {
        Registration = newImplementation;
    }

    public override string ToString() {
        return $"{Registration.GetRegistration().Name} to {Registration.GetImplementation().Name}";
    }
}