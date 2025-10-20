using System.Windows;
using Comp_v4.CompCard;
using Comp_v4.CompCard._Installers;
using Comp_v4.CompCard.Entities;
using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Entities.InputHandlers;
using Comp_v4.NomDict.Installers;
using Comp_v4.NomDict.Operations.Actions.Components;
using Comp_v4.NomDict.View;
using Comp.Db;
using DI;
using Utils.WPF;

namespace Comp_v4.Installers;

public partial class App : Application
{
    protected readonly AreopagContainer _rootContainer;
    protected readonly Dictionary<Type, AreopagContainer> _subContainers = new();
    
    public App() {
        _rootContainer = new AreopagContainer();
        new AppDbContextInstaller().Install(_rootContainer);
        _rootContainer.Add<IWindowOrderLocator>().To<WindowOrderLocator>().AsSingleton();
        _rootContainer.Add<CardComponentManager>().AsSingleton()
                      .UsingFactoryMethod(() => new CardComponentManager(_subContainers[typeof(CompCardWindow)], 
                                                                         _rootContainer.Resolve<IWindowOrderLocator>()));

        var cont = new AreopagContainer();
        new CompCardWindowInstaller(_rootContainer, _subContainers).Install(cont);
        _subContainers[typeof(CompCardWindow)] = cont;


        _subContainers[typeof(NomDictWindow)] = new AreopagContainer() {
            Description = $"Installer of {nameof(NomDictWindow)}"
        };
        _subContainers[typeof(NomDictWindow)].Add<IWindowOrderLocator>()
                                             .To<WindowOrderLocator>()
                                             .AsSingleton()
                                             .UsingFactoryMethod(() => _rootContainer.Resolve<IWindowOrderLocator>());
        
        _subContainers[typeof(NomDictWindow)].Add<AppDbContext>()
                                             .AsSingleton()
                                             .UsingFactoryMethod(() => _rootContainer.Resolve<AppDbContext>());
        
        _subContainers[typeof(NomDictWindow)].Add<CardComponentManager>()
                                             .AsSingleton()
                                             .UsingFactoryMethod(() => _rootContainer.Resolve<CardComponentManager>());
        
        _subContainers[typeof(NomDictWindow)].Add<DataGridInputHandler>()
                                             .AsScoped<NomDictWindow>();
        
        var ndInst = new NomDictInstaller();
        ndInst.Install(_subContainers[typeof(NomDictWindow)]);
    }

    protected override async void OnStartup(StartupEventArgs e) {
        await _rootContainer.Resolve<DatabaseInitializer>().InitializeAsync();

        var subContainer = _subContainers[typeof(NomDictWindow)];
        var window = subContainer.BeginScope<NomDictWindow>();
        _rootContainer.Resolve<IWindowOrderLocator>().RegisterWindow(window);
        window.Closed += (_, _) => subContainer.ReleaseScope<NomDictWindow>();
        _subContainers[typeof(NomDictWindow)].Instantiate<AddCategoryAction, DeleteCategoryAction, UpdateCategoryNameAction, DataGridInputHandler>();
        _subContainers[typeof(NomDictWindow)].Instantiate<AddComponentAction>();
        window.Show();
    }

    protected override async void OnExit(ExitEventArgs e) {
        base.OnExit(e);
    }

    protected void OpenCardComponentWindow(object? parameter) {
        /*var container = _subContainers[typeof(CompCardWindow)];
        var window = container.BeginScope<CompCardWindow>();
        window.Closed += (_, __) => container.ReleaseScope<CompCardWindow>();
        window.Show();*/
    }
}