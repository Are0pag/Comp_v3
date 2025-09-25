using System.Windows;
using Comp_v4.CompCard;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows;
using Comp_v4.TableWindows.ConditionalDesignation;
using Comp_v4.TableWindows.ConditionalDesignation.Overrided;
using Comp_v4.TableWindows.Manufacturers;
using Comp_v4.TableWindows.Manufacturers.Overrided;
using Comp_v4.TableWindows.MeasurementUnits;
using Comp.Db;
using Comp.ModelData.TechnicalItems;
using WPF.Services;
using WPF.Templates.TableWindow.v1.Entities.InputHandlers;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.Installers;

public partial class App : Application
{
    protected readonly AreopagContainer _rootContainer;
    protected readonly Dictionary<Type, AreopagContainer> _subContainers = new();
    
    public App() {
        _rootContainer = new AreopagContainer();
        var appDbContextInstaller = new AppDbContextInstaller();
        appDbContextInstaller.Install(_rootContainer);

        InstallTableWindowScope<CondDesignTableWindow>(new TableWindowInstaller<CondDesignTableWindow, ConditionalDesignation, CdValidator, CdFilter>());
        InstallTableWindowScope<ManufacturersTableWindow>(new TableWindowInstaller<ManufacturersTableWindow, Manufacturer, mValidator, mFilter>());
        InstallTableWindowScope<MeasurementUnitTableWindow>(new TableWindowInstaller<MeasurementUnitTableWindow, MeasurementUnit, muValidator, muFilter>());
    }

    protected override async void OnStartup(StartupEventArgs e) {
        new CompCardWindow(new CompCardVm(), 
                           new CdFieldVm(OpenTableWindow<CondDesignTableWindow, ConditionalDesignation>),
                           new ManFieldVm(OpenTableWindow<ManufacturersTableWindow, Manufacturer>),
                           new MuFieldVm(OpenTableWindow<MeasurementUnitTableWindow, MeasurementUnit>)
                          ).Show();
    }

    protected override async void OnExit(ExitEventArgs e) {
        base.OnExit(e);
    }

    protected void InstallTableWindowScope<TWindow>(AbstractInstaller tableWindowInstaller) 
        where TWindow : Window, IDisposable
    {
        var cdSubContainer = new AreopagContainer();
        cdSubContainer.Add<AppDbContext>().AsSingleton().UsingFactoryMethod(() => _rootContainer.Resolve<AppDbContext>());
        tableWindowInstaller.Install(cdSubContainer);
        _subContainers[typeof(TWindow)] = cdSubContainer;
    }
    
    protected void OpenTableWindow<TWindow, TData>()
        where TWindow : Window, IDisposable
        where TData : class, IDbEntity, new() 
    {
        var contextContainer = _subContainers[typeof(TWindow)];
        var window = contextContainer.BeginScope<TWindow>();
        window.Closed += (sender, args) => {
            contextContainer.ReleaseScope<TWindow>();
        };
        contextContainer
           .Instantiate<ActionStackTracker,
                PersistenceManager<TWindow, TData>,
                TableCommandBinder<TWindow, TData>,
                ActionFilter<TWindow, TData, FiltersVmBase>>();
        window.Show();
    }
}