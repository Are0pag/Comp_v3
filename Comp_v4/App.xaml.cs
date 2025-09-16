using System.Windows;
using Comp_v4.Entities;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
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

public interface ICondDesServiceScope : IServiceScope {}

public partial class App : Application
{
    public static IHost Host { get; protected set; }
    protected IServiceScope _mainScope;


    /* TODO */
    protected static readonly Dictionary<Type, IServiceScope> _scopes = new Dictionary<Type, IServiceScope>();
    public static ICondDesServiceScope TestScope { get; protected set; }

    public static IServiceScope ProvideTransientCommandInScope<T>() where T : IServiceScope {
        return _scopes[typeof(T)];
    }

    public App() {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().
                         ConfigureServices((hostContext, s /* services */) => {
                             
                             s.AddDbContext<AppDbContext>(options => {
                                 options.UseSqlite(DbConfig.ConnectionString);
                             });
                             s.AddTransient<IRepository<ConditionalDesignation>, ConditionalDesignationRepository>();
                             s.AddTransient<IConditionalDesignationRepository, ConditionalDesignationRepository>();

                             s.AddSingleton<Validator>();

                             s.AddTransient<DataGridPropertyRestoreService<ConditionalDesignation>>();
                            
                             s.AddSingleton<IModuleCommandScheduler, ModuleCommandScheduler>();

                             s.AddSingleton<DataGridViewModel>();
                             s.AddSingleton<FiltersVm>();
                             s.AddScoped<ModuleContext>();

                             s.AddScoped<ActionStartAddingNewItem>();
                             s.AddScoped<ActionAddItem>();
                             s.AddScoped<ActionUpdateItem>();
                             s.AddScoped<ActionDeleteItem>();
                             s.AddScoped<ActionSave>();
                             
                             s.AddSingleton<CellStateIdle>();
                             s.AddSingleton<CellStateUpdate>();
                             s.AddSingleton<CellStateAddItem>();
                             s.AddSingleton<BaseCellState>(provider => provider.GetRequiredService<CellStateIdle>());
                             s.AddSingleton<BaseCellState>(provider => provider.GetRequiredService<CellStateUpdate>());
                             s.AddSingleton<BaseCellState>(provider => provider.GetRequiredService<CellStateAddItem>());
                             s.AddSingleton<Cell>(provider => {
                                                   var states = provider.GetServices<BaseCellState>();
                                                   var initialState = provider.GetService<CellStateIdle>();
                                                   return new Cell(states, initialState!);
                                               });
                             

                             s.AddScoped<ButtonVmAddItem>();
                             s.AddSingleton<ButtonVmSave>();
                             s.AddSingleton<ButtonVmDeleteItem>();
                             
                             s.AddTransient<TargetWindow>();

                         }).Build();
    }
    protected override async void OnStartup(StartupEventArgs e) {
        await Host.StartAsync();
        
        // Создаем БД при старте (используем scope)
        using (var scope = Host.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
        } 
        
        
        _mainScope = Host.Services.CreateScope();
        var scheduler = Host.Services.GetRequiredService<IModuleCommandScheduler>();
        new ActionStackTracker(scheduler);
        new PersistenceManager(scheduler, Host.Services.GetRequiredService<ActionSave>());
        new TableCommandBinder(Host.Services.GetRequiredService<ActionStartAddingNewItem>(), Host.Services.GetRequiredService<ActionDeleteItem>());

        var filtersVm = Host.Services.GetRequiredService<FiltersVm>();
        var mContext = Host.Services.GetRequiredService<ModuleContext>();
        var cell = Host.Services.GetRequiredService<Cell>();
        
        new ActionFilter(scheduler, mContext, filtersVm, cell);
        
        /*var scheduler = _mainScope.ServiceProvider.GetRequiredService<IModuleCommandScheduler>();
        new ActionStackTracker(scheduler);*/
        var mainWindow = _mainScope.ServiceProvider.GetRequiredService<TargetWindow>(); 
        mainWindow.Closed += (_, _) => _mainScope?.Dispose();
        mainWindow.Show();
        
        
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        _mainScope?.Dispose();
        await Host.StopAsync();
        Host.Dispose();
        base.OnExit(e);
    }
}