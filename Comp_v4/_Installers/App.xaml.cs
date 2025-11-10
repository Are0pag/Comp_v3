using System.Windows;
using Comp_v4.CompCard._Installers;
using Comp_v4.Entry;
using Comp_v4.Entry._Installers;
using Comp_v4.Entry.Actions;
using Comp_v4.NomDict.Installers;
using Comp_v4.TableWindows.Counterparties._Installers;
using Comp_v4.TableWindows.SupplierOrders.Installers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils.WPF;

namespace Comp_v4.Installers;

public partial class App : Application
{
    protected static IHost _appHost;

    public App() {
        _appHost = Host.CreateDefaultBuilder()
                       .ConfigureServices((hostContext, services) => {
                            services.AddSingleton<IWindowOrderLocator, WindowOrderLocator>();
                            
                            services.RegisterDb();
                            services.InstallEntry();
                            services.RegisterSupplierOrders();
                            services.RegisterCounterparties();
                            
                            services.RegisterNomDict();
                            services.RegisterCardComp();

                        }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e) {
        await _appHost.StartAsync();
        
        //_ = _appHost.Services.GetRequiredService<OpenNomDictAction>();
        _ = _appHost.Services.GetRequiredService<OpenSupplierOrdersAction>();
        
        var mainWindow = _appHost.Services.GetRequiredService<EntryWindow>();
        mainWindow.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        await _appHost.StopAsync();
        _appHost.Dispose();
        base.OnExit(e);
    }
}