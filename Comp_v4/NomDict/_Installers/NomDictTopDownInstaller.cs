using Comp_v4.CompCard._Installers;
using Comp_v4.Installers;
using Comp_v4.NomDict.Entities.InputHandlers;
using Comp_v4.NomDict.View;
using Comp.Db;
using DI;
using DI.Contracts;
using Utils.WPF;

namespace Comp_v4.NomDict.Installers;

public class NomDictTopDownInstaller : ITopDownInstaller
{
    public AreopagContainer InstallFrom(AreopagContainer parentContainer, AreopagContainer childContainer) {
        if (parentContainer is not RootContainer rootContainer || childContainer is not NomDictContainer nomDictContainer)
            throw new InvalidOperationException();
        
        nomDictContainer.Add<IWindowOrderLocator>()
                        .To<WindowOrderLocator>()
                        .AsSingleton()
                        .UsingFactoryMethod(() => rootContainer.Resolve<IWindowOrderLocator>());
        
        nomDictContainer.Add<AppDbContext>()
                        .AsSingleton()
                        .UsingFactoryMethod(() => rootContainer.Resolve<AppDbContext>());
        
        nomDictContainer.Add<CardComponentManager>()
                        .AsSingleton()
                        .UsingFactoryMethod(() => rootContainer.Resolve<CardComponentManager>());
        
        nomDictContainer.Add<DataGridInputHandler>()
                        .AsScoped<NomDictWindow>().EnforceInstantiateOnBegin();
        
        return childContainer;
    }
}