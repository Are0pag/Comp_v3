using System.IO;
using System.Windows;
using Comp.ModelData.TechnicalItems;
using Comp.Db;
using Comp.Db.Contracts;
using Comp_v3.Front.DataGrid.CondDesign;
using Comp.Db.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v3;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App() {
        AppHost = Host.CreateDefaultBuilder().
                       ConfigureServices((hostContext, services) => {
                           services.AddDbContext<AppDbContext>(options => {
                               const string dbName = "comp.db";
                               var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                               var connectionString = $"data source={Path.Combine(folderPath, dbName)}";
                               options.UseSqlite(connectionString);
                           });
                            
                           services.AddTransient<IRepository<ConditionalDesignation>, ConditionalDesignationRepository>();
                           services.AddTransient<IConditionalDesignationRepository, ConditionalDesignationRepository>();

                           // Регистрируем ViewModel и окна
                           services.AddTransient<Front.DataGrid.CondDesign.MainVm>();
                           services.AddTransient<MainW>();

                       }).Build();
    }
    protected override async void OnStartup(StartupEventArgs e) {
        await AppHost!.StartAsync();
        
        // Создаем БД при старте (используем scope)
        using (var scope = AppHost.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
        }

        // Запускаем главное окно
        var mainWindow = AppHost.Services.GetRequiredService<MainW>();
        mainWindow.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}