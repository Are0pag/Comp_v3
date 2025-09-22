namespace WPF.Services;

public interface IRegistrationBuilder
{
    RegistrationProxy Registration { get; }

    object Resolve(AreopagContainer container);
}