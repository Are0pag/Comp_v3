namespace WPF.Services;

public interface IRegistrationBuilder
{
    RegistrationProxy Registration { get; }

    public Func<object>? FactoryResolve { get; set; }
    
    object Resolve(AreopagContainer container);
    
    void OverrideImplementation(RegistrationProxy newImplementation);
}