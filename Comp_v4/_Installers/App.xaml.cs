using System.Windows;
using Comp_v4.CompCard;
using Comp_v4.CompCard._Installers;
using Comp_v4.CompCard.Entities;
using Comp_v4.Entry;
using Comp_v4.Entry._Installers;
using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Entities.InputHandlers;
using Comp_v4.NomDict.Installers;
using Comp_v4.NomDict.Operations.Actions.Components;
using Comp_v4.NomDict.View;
using Comp.Db;
using DI;
using Utils.WPF;

namespace Comp_v4.Installers;

/* метод  HaveSourceContainer(cont) */
public class RootContainer : AreopagContainer {}
public class EntryContainer : AreopagContainer {}
public class NomDictContainer : AreopagContainer {}

public class CounterpartyFormContainer : AreopagContainer {}
public class CounterpartyTableContainer : AreopagContainer {}

public partial class App : Application
{
    protected readonly AreopagContainer _rootContainer;
    protected readonly Dictionary<Type, AreopagContainer> _subContainers = new();
    
    public App() {
        _rootContainer = new RootContainer();
        new AppDbContextInstaller().Install(_rootContainer);
        _rootContainer.Add<IWindowOrderLocator>().To<WindowOrderLocator>().AsSingleton();
        _rootContainer.Add<CardComponentManager>().AsSingleton()
                      .UsingFactoryMethod(() => new CardComponentManager(_subContainers[typeof(CompCardWindow)], 
                                                                         _rootContainer.Resolve<IWindowOrderLocator>()));


        
        var cont = new AreopagContainer();
        new CompCardWindowInstaller(_rootContainer, _subContainers).Install(cont);
        _subContainers[typeof(CompCardWindow)] = cont;


        var ndc = new NomDictContainer() {
            Description = $"Installer of {nameof(NomDictWindow)}"
        };
        _subContainers[typeof(NomDictWindow)] = ndc;
        var entryCont = new EntryContainer() {
            Description = $"Installer of Entry Window"
        };
        _subContainers[typeof(EntryWindow)] = entryCont;
        
        new EntrySelfInstaller().InstallSelf(entryCont);
        new EntryTopDownInstaller().InstallFrom(_rootContainer, entryCont);
        
        new NomDictInstaller().Install(ndc);
        new NomDictTopDownInstaller().InstallFrom(_rootContainer, ndc);

        _rootContainer.Add<NomDictContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => ndc);
        
        _rootContainer.Add<EntryContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => entryCont);
    }

    protected override async void OnStartup(StartupEventArgs e) {
        await _rootContainer.Resolve<DatabaseInitializer>().InitializeAsync();
        var window = WindowContextResolver.ResolveWindow<EntryWindow>(_subContainers[typeof(EntryWindow)]);
        _rootContainer.Resolve<IWindowOrderLocator>().RegisterWindow(window);
    }

    protected override async void OnExit(ExitEventArgs e) {
        base.OnExit(e);
    }
}