using System.Windows;
using Comp_v4.CompCard;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows;
using Comp_v4.TableWindows.ConditionalDesignation.Overrided;
using Comp_v4.TableWindows.Manufacturers;
using Comp_v4.TableWindows.Manufacturers.Overrided;
using Comp.Db;
using Comp.ModelData.TechnicalItems;
using WPF.Services;
using WPF.Templates.TableWindow.v1.Entities.InputHandlers;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm.Components;
using Tw = Comp_v4.TableWindows.ConditionalDesignation.TargetWindow;
using Cd = Comp.ModelData.TechnicalItems.ConditionalDesignation;

namespace Comp_v4.Installers;

public partial class App : Application
{
    protected readonly AreopagContainer _rootContainer;
    protected readonly Dictionary<Type, AreopagContainer> _subContainers = new();
    
    public App() {
        _rootContainer = new AreopagContainer();
        var appDbContextInstaller = new AppDbContextInstaller();
        appDbContextInstaller.Install(_rootContainer);

        var cdWindowInstaller = new TableWindowInstaller<Tw, Cd, CdValidator, CdFilter>();
        var cdSubContainer = new AreopagContainer();
        cdWindowInstaller.Install(cdSubContainer);
        cdSubContainer.Add<AppDbContext>().AsSingleton().UsingFactoryMethod(() => _rootContainer.Resolve<AppDbContext>());
        _subContainers[typeof(Tw)] = cdSubContainer;

        var mWindowInstaller = new TableWindowInstaller<ManufacturersTableWindow, Manufacturer, mValidator, mFilter>();
        var mSubContainer = new AreopagContainer();
        mSubContainer.Add<AppDbContext>().AsSingleton().UsingFactoryMethod(() => _rootContainer.Resolve<AppDbContext>());
        mWindowInstaller.Install(mSubContainer);
        
        _subContainers[typeof(ManufacturersTableWindow)] = mSubContainer;
    }

    protected override async void OnStartup(StartupEventArgs e) {
        new CompCardWindow(new CompCardVm(), 
                           new CdFieldVm(() => {
                               var contextContainer = _subContainers[typeof(Tw)];
                               var window = contextContainer.BeginScope<Tw>();
                               window.Closed += (sender, args) => {
                                   contextContainer.ReleaseScope<Tw>();
                               };
                               contextContainer
                                  .Instantiate<ActionStackTracker, PersistenceManager<Tw, Cd>,
                                       TableCommandBinder<Tw, Cd>, ActionFilter<Tw, Cd, FiltersVmBase>>();
                               window.Show();
                           }),
                           new ManFieldVm(() => {
                               var contextContainer = _subContainers[typeof(ManufacturersTableWindow)];
                               var window = contextContainer.BeginScope<ManufacturersTableWindow>();
                               window.Closed += (sender, args) => {
                                   contextContainer.ReleaseScope<ManufacturersTableWindow>();
                               };
                               contextContainer
                                  .Instantiate<ActionStackTracker,
                                       PersistenceManager<ManufacturersTableWindow, Manufacturer>,
                                       TableCommandBinder<ManufacturersTableWindow, Manufacturer>,
                                       ActionFilter<ManufacturersTableWindow, Manufacturer, FiltersVmBase>>();
                               window.Show();
                           })
                          ).Show();
    }

    protected override async void OnExit(ExitEventArgs e) {
        base.OnExit(e);
    }
}