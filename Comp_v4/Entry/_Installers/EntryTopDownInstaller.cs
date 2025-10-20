using Comp_v4.Installers;
using DI;
using DI.Contracts;

namespace Comp_v4.Entry._Installers;

public class EntryTopDownInstaller : ITopDownInstaller
{
    public AreopagContainer InstallFrom(AreopagContainer parentContainer, AreopagContainer childContainer) {
        if (parentContainer is not RootContainer rootContainer || childContainer is not EntryContainer entryContainer) 
            throw new ArgumentException();

        entryContainer.Add<NomDictContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => {
                           if (rootContainer.Resolve<NomDictContainer>() is not { } nomDictContainer)
                               throw new ArgumentException();
                           return nomDictContainer;
                       });
        
        return childContainer;
    }
}