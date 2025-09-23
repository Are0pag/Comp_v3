using System.Windows;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Commands.Filtering;
using Comp.Db;
using Comp.Db.Contracts;
using Infrastructure.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPF.Services;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.Validation;
using WPF.Templates;
using WPF.Templates.TableWindow.States;
using WPF.Templates.TableWindow.Vm;
using WPF.Templates.TableWindow.Vm.Components;

using Tw = Comp_v4.TargetWindow;
using Cd = Comp.ModelData.TechnicalItems.ConditionalDesignation;

namespace Comp_v4;


public partial class App : Application
{
    public static IHost Host { get; protected set; }
    
    public static WindsorContainer RootWindsorContainer { get; protected set; }
    
    protected IServiceScope _mainScope;
    protected List<IDisposable> _disposable;
    
    protected IDisposable _appScope;

    protected readonly AreopagContainer _rootContainer;
    
    public App() {
        /*RootWindsorContainer = new WindsorContainer();*/
        // adds and configures all components using WindsorInstallers from executing assembly
        /*RootWindsorContainer.Install(FromAssembly.This());*/
        
        //BuildByMicrosoft();
        
        _rootContainer = new AreopagContainer();
        _rootContainer.Install();
    }

    protected override async void OnStartup(StartupEventArgs e) {
        /*_appScope = RootWindsorContainer.BeginScope();
        var mainWindow = RootWindsorContainer.Resolve<TargetWindow>();

        mainWindow.Closed += (_, _) => {
            _appScope.Dispose();
        };
        mainWindow.Show();*/

        var window = _rootContainer.BeginScope<Tw>();

        window.Closed += (sender, args) => {
            _rootContainer.ReleaseScope<Tw>();
        };

        _rootContainer.Instantiate<ActionStackTracker, PersistenceManager<Tw, Cd>, TableCommandBinder<Tw, Cd>, ActionFilter<Tw, Cd, FiltersVmCd>>();
        
        window.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        base.OnExit(e);
    }

    /// <summary>
    /// This method exists because DI do not create that instances
    /// </summary>
    protected virtual void CreateRequiredInstancesManually() {
        //return;
        var scheduler = _mainScope.ServiceProvider.GetRequiredService<IDataGridCommandScheduler>();
        var filtersVm = _mainScope.ServiceProvider.GetRequiredService<FiltersVmCd>();
        var mContext = _mainScope.ServiceProvider.GetRequiredService<ModuleContext<Tw, Cd>>();
        var cell = _mainScope.ServiceProvider.GetRequiredService<Cell<Tw, Cd>>();
        var comFactory = _mainScope.ServiceProvider.GetRequiredService<ICommandFactory>();
        
        _disposable = new List<IDisposable>() {
            new ActionStackTracker(scheduler),
            new PersistenceManager<Tw, Cd>(scheduler, _mainScope.ServiceProvider.GetRequiredService<ActionSave<Tw, Cd>>()),
            new TableCommandBinderFilteringCompatible<Tw, Cd>(
                                                              _mainScope.ServiceProvider.GetRequiredService<ActionStartAddingNewItem<Tw, Cd>>(), 
                                                              _mainScope.ServiceProvider.GetRequiredService<ActionDeleteItem<Tw, Cd>>()
                                                             ),
            new ActionFilter<Tw, Cd, FiltersVmCd>(scheduler, mContext, comFactory, filtersVm, cell)
        };
    }

    private static void BuildByMicrosoft() {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().
                         ConfigureServices((hostContext, s /* services */) => {
                             
                             s.AddDbContext<AppDbContext>(options => {
                                 options.UseSqlite(DbConfig.ConnectionString);
                             });
                             s.AddTransient<IRepository<Cd>, ConditionalDesignationRepository>();

                             s.AddSingleton<System.Windows.Threading.Dispatcher>(provider => App.Current.Dispatcher);

                             s.AddSingleton<ValidatorBase<Cd>, Validator>();
                             s.AddSingleton<ICommandFactory, CommandFactory>(provider => new CommandFactory(provider));

                             s.AddSingleton<IDataGridCommandScheduler, DataGridCommandScheduler>();
                             s.AddTransient<DataGridPropertyRestoreService<Cd>>();
                             s.AddSingleton<IFilter<Cd, FiltersVmCd>, Filter>();

                             s.AddSingleton<DataGridViewModel<Cd>>();
                             s.AddSingleton<FiltersVmCd>();

                             s.AddSingleton<ModuleContext<Tw, Cd>>();

                             s.AddScoped<ActionStartAddingNewItem<Tw, Cd>>();
                             s.AddScoped<ActionAddItem<Tw, Cd>>();
                             s.AddScoped<ActionUpdateItem<Tw, Cd>>();
                             s.AddScoped<ActionDeleteItem<Tw, Cd>>();
                             s.AddScoped<ActionSave<Tw, Cd>>();
                             
                             s.AddSingleton<CellStateIdle<Tw, Cd>>();
                             s.AddSingleton<CellStateUpdate<Tw, Cd>>();
                             s.AddSingleton<CellStateAddItem<Tw, Cd>>();
                             s.AddSingleton<BaseCellState<Tw, Cd>>(provider => provider.GetRequiredService<CellStateIdle<Tw, Cd>>());
                             s.AddSingleton<BaseCellState<Tw, Cd>>(provider => provider.GetRequiredService<CellStateUpdate<Tw, Cd>>());
                             s.AddSingleton<BaseCellState<Tw, Cd>>(provider => provider.GetRequiredService<CellStateAddItem<Tw, Cd>>());
                             s.AddSingleton<Cell<Tw, Cd>>(provider => {
                                 var states = provider.GetServices<BaseCellState<Tw, Cd>>();
                                 var initialState = provider.GetService<CellStateIdle<Tw, Cd>>();
                                 return new Cell<Tw, Cd>(states, initialState!);
                             });
                             
                             
                             s.AddScoped<ButtonVmAddItem<Tw, Cd>>();
                             s.AddSingleton<ButtonVmSave<Tw, Cd>>();
                             s.AddSingleton<ButtonVmDeleteItem<Tw, Cd>>();
                             
                             s.AddTransient<Tw>();

                         }).Build();
    }
}