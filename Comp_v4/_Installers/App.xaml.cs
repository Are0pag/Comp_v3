using System.Windows;
using Comp_v4.CompCard;
using Comp_v4.CompCard._Installers;
using Comp_v4.Entry;
using Comp_v4.Entry._Installers;
using Comp_v4.NomDict.Installers;
using Comp_v4.NomDict.View;
using Comp.Db;
using DI;
using Utils.WPF;

namespace Comp_v4.Installers;

public class RootContainer : AreopagContainer {}
public class EntryContainer : AreopagContainer {}
public class NomDictContainer : AreopagContainer {}

public class CounterpartyFormContainer : AreopagContainer {}
public class CounterpartyTableContainer : AreopagContainer {}

public class SupplierOrderFormContainer : AreopagContainer {}
public class SupplierOrderTableContainer : AreopagContainer {}

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
        _rootContainer.Add<EntryContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => entryCont);
        
        new NomDictInstaller().Install(ndc);
        new NomDictTopDownInstaller().InstallFrom(_rootContainer, ndc);
        _rootContainer.Add<NomDictContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => ndc);
        
        
        var ctc = new CounterpartyTableContainer();
        new Comp_v4.TableWindows.Counterparties.Table._Installers.SelfInstaller().InstallSelf(ctc);
        _rootContainer.Add<CounterpartyTableContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => ctc);
        
        var cfc = new CounterpartyFormContainer();
        new TableWindows.Counterparties.Form._Installers.FormSelfInstaller().InstallSelf(cfc);
        new TableWindows.Counterparties.Form._Installers.CounterpartyFormTopDownInstaller().InstallFrom(_rootContainer, cfc);
        _rootContainer.Add<CounterpartyFormContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => cfc);
        
        var sotc = new SupplierOrderTableContainer();
        new TableWindows.SupplierOrders.Table._Installers.SelfInstaller().InstallSelf(sotc);
        new TableWindows.SupplierOrders.Table._Installers.TopDownInstaller().InstallFrom(_rootContainer, sotc);
        _rootContainer.Add<SupplierOrderTableContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => sotc);
        
        var sofc = new SupplierOrderFormContainer();
        new TableWindows.SupplierOrders.Form._Installers.SelfInstaller().InstallSelf(sofc);
        new TableWindows.SupplierOrders.Form._Installers.TopDownInstaller().InstallFrom(_rootContainer, sofc);
        _rootContainer.Add<SupplierOrderFormContainer>()
                      .AsSingleton()
                      .UsingFactoryMethod(() => sofc);

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