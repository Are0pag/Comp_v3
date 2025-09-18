using System.Windows;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Templates;
using WPF.Templates.TableWindow.States;
using WPF.Templates.TableWindow.Vm;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4;


public partial class App : Application
{
    public static IHost Host { get; protected set; }
    protected IServiceScope _mainScope;
    protected List<IDisposable> _disposable;
    
    public App() {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().
                         ConfigureServices((hostContext, s /* services */) => {
                             
                             s.AddDbContext<AppDbContext>(options => {
                                 options.UseSqlite(DbConfig.ConnectionString);
                             });
                             s.AddTransient<IRepository<ConditionalDesignation>, ConditionalDesignationRepository>();

                             System.Windows.Threading.Dispatcher disp = App.Current.Dispatcher;
                             var d = App.Current.Dispatcher;
                             
                             s.AddSingleton<CommandFactory>(provider => new CommandFactory(provider));
                             s.AddSingleton<Validator>();

                             s.AddSingleton<IDataGridCommandScheduler, DataGridCommandScheduler>();
                             s.AddTransient<DataGridPropertyRestoreService<ConditionalDesignation>>();

                             s.AddSingleton<DataGridViewModel<ConditionalDesignation>>();
                             s.AddSingleton<FiltersVmCd>();

                             s.AddSingleton<ModuleContext<TargetWindow, ConditionalDesignation>>();

                             s.AddScoped<ActionStartAddingNewItem<TargetWindow, ConditionalDesignation>>();
                             s.AddScoped<ActionAddItem<TargetWindow, ConditionalDesignation>>();
                             s.AddScoped<ActionUpdateItem<TargetWindow, ConditionalDesignation>>();
                             s.AddScoped<ActionDeleteItem<TargetWindow, ConditionalDesignation>>();
                             s.AddScoped<ActionSave<TargetWindow, ConditionalDesignation>>();
                             
                             s.AddSingleton<CellStateIdle<TargetWindow, ConditionalDesignation>>();
                             s.AddSingleton<CellStateUpdate<TargetWindow, ConditionalDesignation>>();
                             s.AddSingleton<CellStateAddItem<TargetWindow, ConditionalDesignation>>();
                             s.AddSingleton<BaseCellState<TargetWindow, ConditionalDesignation>>(provider => provider.GetRequiredService<CellStateIdle<TargetWindow, ConditionalDesignation>>());
                             s.AddSingleton<BaseCellState<TargetWindow, ConditionalDesignation>>(provider => provider.GetRequiredService<CellStateUpdate<TargetWindow, ConditionalDesignation>>());
                             s.AddSingleton<BaseCellState<TargetWindow, ConditionalDesignation>>(provider => provider.GetRequiredService<CellStateAddItem<TargetWindow, ConditionalDesignation>>());
                             s.AddSingleton<Cell<TargetWindow, ConditionalDesignation>>(provider => {
                                                   var states = provider.GetServices<BaseCellState<TargetWindow, ConditionalDesignation>>();
                                                   var initialState = provider.GetService<CellStateIdle<TargetWindow, ConditionalDesignation>>();
                                                   return new Cell<TargetWindow, ConditionalDesignation>(states, initialState!);
                                               });
                             

                             s.AddScoped<ButtonVmAddItem<TargetWindow, ConditionalDesignation>>();
                             s.AddSingleton<ButtonVmSave<TargetWindow, ConditionalDesignation>>();
                             s.AddSingleton<ButtonVmDeleteItem<TargetWindow, ConditionalDesignation>>();
                             
                             s.AddTransient<TargetWindow>();

                         }).Build();
    }
    
    protected override async void OnStartup(StartupEventArgs e) {
        await Host.StartAsync();
        
        using (var scope = Host.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
        } 
        
        _mainScope = Host.Services.CreateScope();
        CreateRequiredInstancesManually();

        var mainWindow = _mainScope.ServiceProvider.GetRequiredService<TargetWindow>(); 
        mainWindow.Closed += (_, _) => {
            foreach (var d in _disposable) d.Dispose();
            _disposable.Clear();
            _mainScope?.Dispose();
        };
        mainWindow.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        _mainScope?.Dispose();
        await Host.StopAsync();
        Host.Dispose();
        base.OnExit(e);
    }

    /// <summary>
    /// This method exists because DI do not create that instances
    /// </summary>
    protected virtual void CreateRequiredInstancesManually() {
        var scheduler = _mainScope.ServiceProvider.GetRequiredService<IDataGridCommandScheduler>();
        var filtersVm = _mainScope.ServiceProvider.GetRequiredService<FiltersVmCd>();
        var mContext = _mainScope.ServiceProvider.GetRequiredService<ModuleContext<TargetWindow, ConditionalDesignation>>();
        var cell = _mainScope.ServiceProvider.GetRequiredService<Cell<TargetWindow, ConditionalDesignation>>();
        var comFactory = _mainScope.ServiceProvider.GetRequiredService<CommandFactory>();
        
        _disposable = new List<IDisposable>() {
            new ActionStackTracker(scheduler),
            new PersistenceManager<TargetWindow, ConditionalDesignation>(scheduler, _mainScope.ServiceProvider.GetRequiredService<ActionSave<TargetWindow, ConditionalDesignation>>()),
            new TableCommandBinderFilteringCompatible<TargetWindow, ConditionalDesignation>(
                                                      _mainScope.ServiceProvider.GetRequiredService<ActionStartAddingNewItem<TargetWindow, ConditionalDesignation>>(), 
                                                      _mainScope.ServiceProvider.GetRequiredService<ActionDeleteItem<TargetWindow, ConditionalDesignation>>()
                                                      ),
            new ActionFilter<TargetWindow, ConditionalDesignation, FiltersVmCd>(scheduler, mContext, comFactory, filtersVm, cell)
        };
    }
}