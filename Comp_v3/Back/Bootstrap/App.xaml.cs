using System.Windows;
using System.Windows.Controls;
using Comp_v3.Back.Bootstrap.ServiceCollectionExtensions.Db;
using Comp.Db;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Grid.States;
using Comp_v3.Front.DataGrid.CondDesign.GridButtonsPanel;
using Comp_v3.Front.DataGrid.CondDesign.Window.States;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Base;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WPF.Services.UserActionsHandling.InputKey;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.View.AutoNavigation.Focusing;
using CognDesignGridWindow = Comp_v3.Front.DataGrid.CondDesign.Window.CognDesignGridWindow;


namespace Comp_v3;

public partial class App : Application
{
    public static IHost Host { get; protected set; }
    protected IServiceScope _mainScope;

    public App() {
        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder().
                         ConfigureServices((hostContext, services) => {
                             services.RegisterConditionalDesignationsTable();

                             services.AddSingleton<CursorPositionService<DataGrid>, DataGridCursorPositionService>();

                             services.AddScoped<IPropertyValueRestoreService<ConditionalDesignation>, DataGridPropertyRestoreService<ConditionalDesignation>>();
                             services.AddScoped<HeterochromicCommandScheduler>();
                             services.AddScoped<CommonUndoRedoHotKeysService>();
                             services.AddScoped<EnhancedDataGridCursorPositionService>();
                           
                             services.AddScoped<StateDgEditing>();
                             services.AddScoped<StateDgCreatingNewItem>();
                             services.AddScoped<StateDataGrid, StateDgEditing>();         // как интерфейс
                             services.AddScoped<StateDataGrid, StateDgCreatingNewItem>(); // как интерфейс
                             services.AddScoped<StateProviderDg>(provider => {
                                 var states = provider.GetServices<StateDataGrid>();
                                 var initialState = provider.GetService<StateDgEditing>();
                                 return new StateProviderDg(states, initialState);
                             });

                             services.AddScoped<StateWaitingToInputIntoNewItem>();
                             services.AddScoped<StateEditableGrid>();
                             services.AddScoped<Provider>();
                           
                             services.AddScoped<CognDesignGridVm>();
                             services.AddScoped<AddNewItemButVm>();
                             services.AddScoped<SaveChangesButVm>();
                             services.AddScoped<DeleteItemButVm>();
                           
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
        await Host.StartAsync();
        
        // Создаем БД при старте (используем scope)
        using (var scope = Host.Services.CreateScope()) {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await context.Database.EnsureCreatedAsync();
        } 
        
        _mainScope = Host.Services.CreateScope();
        var mainWindow = _mainScope.ServiceProvider.GetRequiredService<CognDesignGridWindow>(); /*var mainWindow = AppHost.Services.GetRequiredService<CognDesignGridWindow>();*/
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