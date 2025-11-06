using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4._Installers;

public class WindowsManager
{
    protected readonly Dictionary<Type, IServiceScopeFactory> _scopeFactories = new();
    
    
}