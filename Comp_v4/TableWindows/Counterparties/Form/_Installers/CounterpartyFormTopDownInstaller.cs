using DI;
using DI.Contracts;

namespace Comp_v4.TableWindows.Counterparties._Installers;

public class CounterpartyFormTopDownInstaller : ITopDownInstaller
{
    public AreopagContainer InstallFrom(AreopagContainer parentContainer, AreopagContainer childContainer) {
        
        
        return childContainer;
    }
}