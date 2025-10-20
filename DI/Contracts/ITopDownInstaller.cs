namespace DI.Contracts;

public interface ITopDownInstaller
{
    AreopagContainer InstallFrom(AreopagContainer parentContainer, AreopagContainer childContainer);
}