namespace WPF.Services;

public class ResolveArgs
{
    public ResolveArgs(Container container, IRegistrationBuilder? caller = null) {
        Container = container;
        Caller = caller;
    }

    public Container Container { get; }
    public IRegistrationBuilder? Caller { get; }
}