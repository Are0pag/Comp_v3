using System.Windows;
using Comp_v3.Back.Bootstrap.ServiceCollectionExtensions.Db;
using Comp.Db;
using Comp_v3.Front.DataGrid.CondDesign;
using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Comp_v3.Front.DataGrid.CondDesign.States.DataGrid;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v3;

public partial class App : Application
{
    protected static IHost _appHost;
    protected IServiceScope _mainScope;

    public App() {
        _appHost = Host.CreateDefaultBuilder().
                       ConfigureServices((hostContext, services) => {
                           services.RegisterConditionalDesignationsTable();

                           services.AddScoped<StateDgEditing>();
                           services.AddScoped<StateDgCreatingNewItem>();
                           services.AddScoped<StateProviderDg>();
                           
                           services.AddScoped<CognDesignGridVm>();
                           services.AddScoped<DataGridManageButtonsVm>();
                           services.AddTransient<CognDesignGridWindow>();

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
        await _appHost.StartAsync();
        
        // Создаем БД при старте (используем scope)
        using (var scope = _appHost.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
        } 
        
        _mainScope = _appHost.Services.CreateScope();
        var mainWindow = _mainScope.ServiceProvider.GetRequiredService<CognDesignGridWindow>(); /*var mainWindow = AppHost.Services.GetRequiredService<CognDesignGridWindow>();*/
        mainWindow.Closed += (_, _) => _mainScope?.Dispose();
        mainWindow.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        _mainScope?.Dispose();
        await _appHost.StopAsync();
        _appHost.Dispose();
        base.OnExit(e);
    }
}