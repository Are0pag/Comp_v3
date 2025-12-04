using System.Windows;
using Comp_v4._Installers.ServiceCollectionExtentions;
using Comp_v4.CompCard._Installers;
using Comp_v4.Entry;
using Comp_v4.Entry._Installers;
using Comp_v4.Entry.Actions;
using Comp_v4.NomDict.Installers;
using Comp_v4.TableWindows.Analogs._Installers;
using Comp_v4.TableWindows.ConditionalDesignation;
using Comp_v4.TableWindows.ConditionalDesignation.Overrided;
using Comp_v4.TableWindows.Counterparties._Installers;
using Comp_v4.TableWindows.GenericParametersSets;
using Comp_v4.TableWindows.Manufacturers;
using Comp_v4.TableWindows.Manufacturers.Overrided;
using Comp_v4.TableWindows.MeasurementUnits;
using Comp_v4.TableWindows.SupplierOrders.Installers;
using Comp_v4.TableWindows.TypeSizes;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Utils.WPF;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

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
                            services.RegisterOrderPositions();
                            
                            services.RegisterNomDict();
                            services.RegisterCardComp();
                            services.RegisterAnalogs();

                            services.RegisterCardCompButtonsToResolveTemplateWindows();

                            services.RegisterTableWindows<CondDesignTableWindow, ConditionalDesignation, CdValidator, CdFilter>();
                            services.AddSingleton<IFilter<ConditionalDesignation, FiltersVmBase>, CdFilter>();

                            services.RegisterTableWindows<ManufacturersTableWindow, Manufacturer, mValidator, mFilter>();
                            services.AddSingleton<IFilter<Manufacturer, FiltersVmBase>, mFilter>();
                            
                            services.RegisterTableWindows<MeasurementUnitTableWindow, MeasurementUnit, muValidator, muFilter>();
                            services.AddSingleton<IFilter<MeasurementUnit, FiltersVmBase>, muFilter>();
                            
                            services.RegisterTableWindows<TypeSizesTableWindow, TypeSize, tsValidator, tsFilter>();
                            services.AddSingleton<IFilter<TypeSize, FiltersVmBase>, tsFilter>();
                            
                            services.RegisterTableWindows<GenericParametersSetsWindow, GenericParametersSet, gpsValidator, gpsFilter>();
                            services.AddSingleton<IFilter<GenericParametersSet, FiltersVmBase>, gpsFilter>();
                            
                            services.RegisterTypeSizes();
                        }).Build();
    }

    protected override async void OnStartup(StartupEventArgs e) {
        await _appHost.StartAsync();
        
        _ = _appHost.Services.GetRequiredService<OpenNomDictAction>();
        _ = _appHost.Services.GetRequiredService<OpenSupplierOrdersAction>();
        
        var mainWindow = _appHost.Services.GetRequiredService<EntryWindow>();
        _appHost.Services.GetRequiredService<IWindowOrderLocator>().RegisterWindow(mainWindow);
        mainWindow.Show();
        
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        await _appHost.StopAsync();
        _appHost.Dispose();
        base.OnExit(e);
    }
}