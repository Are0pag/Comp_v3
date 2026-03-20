namespace DI.Contracts;

public interface IInstallationContext
{
    AreopagContainer SetContext(AreopagContainer targetContainer, params Type[] types);
}