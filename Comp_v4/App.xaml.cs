using System.Windows;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
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
                            
                             s.AddScoped<HeterochromicCommandScheduler>();

                             s.AddScoped<DataGridViewModel>();
                             s.AddScoped<ModuleContext>();

                             s.AddScoped<CellStateIdle>();
                             s.AddScoped<CellStateInput>();
                             /*s.AddScoped<BaseCellState, CellStateIdle>();
                             s.AddScoped<BaseCellState, CellStateInput>();*/
                             s.AddScoped<BaseCellState>(provider => provider.GetRequiredService<CellStateIdle>());
                             s.AddScoped<BaseCellState>(provider => provider.GetRequiredService<CellStateInput>());
                             s.AddScoped<Cell>(provider => {
                                                   var states = provider.GetServices<BaseCellState>();
                                                   var initialState = provider.GetService<CellStateInput>();
                                                   return new Cell(states, initialState!);
                                               });
                             
                             s.AddScoped<ActionAddItem>();
                             s.AddScoped<ButtonVmAddItem>();
                             
                             s.AddTransient<TargetWindow>();

                         }).Build();
    }
    
    /* Также можно явно указать зависимость:
     services.AddSingleton<DataGridManageButtonsVm>(provider =>
        new DataGridManageButtonsVm(
            provider.GetService<IConditionalDesignationRepository>(),
            provider.GetService<CognDesignGridVm>()
    )
     */
    
    protected override async void OnStartup(StartupEventArgs e) {
        await Host.StartAsync();
        
        // Создаем БД при старте (используем scope)
        using (var scope = Host.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
        } 
        
        
        _mainScope = Host.Services.CreateScope();
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