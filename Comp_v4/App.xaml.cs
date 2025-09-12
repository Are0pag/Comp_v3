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

namespace Comp_v4;

public partial class App : Application
{
    public static IHost Host { get; protected set; }
    protected IServiceScope _mainScope;

    public App() {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().
                         ConfigureServices((hostContext, s /* services */) => {
                             
                             s.AddDbContext<AppDbContext>(options => {
                                 options.UseSqlite(DbConfig.ConnectionString);
                             });
                             s.AddTransient<IRepository<ConditionalDesignation>, ConditionalDesignationRepository>();
                             s.AddTransient<IConditionalDesignationRepository, ConditionalDesignationRepository>();

                             s.AddTransient<DataGridPropertyRestoreService<ConditionalDesignation>>();
                            
                             s.AddSingleton<IModuleCommandScheduler, ModuleCommandScheduler>();

                             s.AddSingleton<DataGridViewModel>();
                             s.AddScoped<ModuleContext>();

                             s.AddScoped<ActionAddItem>();
                             s.AddScoped<ActionUpdateItem>();
                             s.AddScoped<ActionSave>();
                             
                             s.AddSingleton<CellStateIdle>();
                             s.AddSingleton<CellStateInput>();
                             /*s.AddScoped<BaseCellState, CellStateIdle>();
                             s.AddScoped<BaseCellState, CellStateInput>();*/
                             s.AddSingleton<BaseCellState>(provider => provider.GetRequiredService<CellStateIdle>());
                             s.AddSingleton<BaseCellState>(provider => provider.GetRequiredService<CellStateInput>());
                             s.AddSingleton<Cell>(provider => {
                                                   var states = provider.GetServices<BaseCellState>();
                                                   var initialState = provider.GetService<CellStateIdle>();
                                                   return new Cell(states, initialState!);
                                               });
                             

                             s.AddScoped<ButtonVmAddItem>();
                             s.AddSingleton<ButtonVmSave>();
                             
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
        new ActionStackTracker(Host.Services.GetRequiredService<IModuleCommandScheduler>());
        new PersistenceManager(Host.Services.GetRequiredService<IModuleCommandScheduler>(), Host.Services.GetRequiredService<ActionSave>());
        new TableCommandBinder();
        
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