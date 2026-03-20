using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected static IHost _appHost;
    protected IServiceScope _mainScope;

    public App() {
        _appHost = Host.CreateDefaultBuilder()
                       .ConfigureServices((hostContext, services) => {
                            services.AddHostedService<Worker>();
                            services.AddScoped<ILogger, Logger>();
                            

                        }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e) {
        await _appHost.StartAsync();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        _mainScope?.Dispose();
        await _appHost.StopAsync();
        _appHost.Dispose();
        base.OnExit(e);
    }
}