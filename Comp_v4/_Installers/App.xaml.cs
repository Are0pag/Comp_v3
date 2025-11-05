using System.Windows;
using Comp_v4.Entry;
using Comp_v4.Entry._Installers;
using Comp_v4.Entry.Actions;
using Comp_v4.TableWindows.Counterparties._Installers;
using Comp_v4.TableWindows.SupplierOrders.Installers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Comp_v4.Installers;

public partial class App : Application
{
    protected static IHost _appHost;

    public App() {
        _appHost = Host.CreateDefaultBuilder()
                       .ConfigureServices((hostContext, services) => {
                            services.RegisterDb();
                            services.InstallEntry();
                            services.RegisterSupplierOrders();
                            services.RegisterCounterparties();

                        }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e) {
        await _appHost.StartAsync();

        var mainWindow = _appHost.Services.GetRequiredService<EntryWindow>();
        
        _ = _appHost.Services.GetRequiredService<OpenNomDictAction>();
        _ = _appHost.Services.GetRequiredService<OpenSupplierOrdersAction>();
        
        mainWindow.Show();

        /*_mainScope = _appHost.Services.CreateScope();
        
        _ = _mainScope.ServiceProvider.GetRequiredService<OpenNomDictAction>();
        _ = _mainScope.ServiceProvider.GetRequiredService<OpenSupplierOrdersAction>();
        
        var mainWindow = _mainScope.ServiceProvider.GetRequiredService<EntryWindow>();
        mainWindow.Closed += (_, _) => _mainScope?.Dispose();*/
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        await _appHost.StopAsync();
        _appHost.Dispose();
        base.OnExit(e);
    }
}