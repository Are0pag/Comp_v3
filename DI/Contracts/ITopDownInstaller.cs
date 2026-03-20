namespace DI.Contracts;

public interface ITopDownInstaller
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parentContainer"></param>
    /// <param name="childContainer"></param>
    /// <returns>Return type is childContainer</returns>
    AreopagContainer InstallFrom(AreopagContainer parentContainer, AreopagContainer childContainer);
}