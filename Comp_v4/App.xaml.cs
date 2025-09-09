using System.Windows;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WPF.Templates;
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

                             s.AddScoped<DataGridViewModel>();

                             s.AddScoped<HeterochromicCommandScheduler>();
                             s.AddScoped<ModuleContext>();
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