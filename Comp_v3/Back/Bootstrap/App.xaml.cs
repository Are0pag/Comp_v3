using System.Windows;
using Comp_v3.Back.Bootstrap.ServiceCollectionExtensions.Db;
using Comp.Db;
using Comp_v3.Front.DataGrid.CondDesign;
using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v3;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App() {
        AppHost = Host.CreateDefaultBuilder().
                       ConfigureServices((hostContext, services) => {
                           services.RegisterConditionalDesignationsTable();
                           
                           services.AddSingleton<CognDesignGridVm>();
                           services.AddTransient<CognDesignGridWindow>();
                           services.AddSingleton<DataGridManageButtonsVm>();

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
        await AppHost!.StartAsync();
        
        // Создаем БД при старте (используем scope)
        using (var scope = AppHost.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
        }

        // Запускаем главное окно
        var mainWindow = AppHost.Services.GetRequiredService<CognDesignGridWindow>();
        mainWindow.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}