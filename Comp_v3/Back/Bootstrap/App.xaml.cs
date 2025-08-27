using System.Windows;
using Comp_v3.Front.DataGrid.CondDesign;
using Comp_v3.NomDict.View;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NomDictWindow = Comp_v3.Front.NomDict.View.NomDictWindow;

namespace Comp_v3;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App() {
        AppHost = Host.CreateDefaultBuilder().
                       ConfigureServices((hostContext, services) => {
                            services.AddNomDictEntities();
                       }).Build();
    }
    protected override async void OnStartup(StartupEventArgs e) {
        await AppHost!.StartAsync();

        /*var startupForm = AppHost.Services.GetRequiredService<NomDictWindow>();
        startupForm.Show();*/
        
        new MainW(new MainVm()).Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}