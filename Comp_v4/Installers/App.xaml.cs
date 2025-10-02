using System.Windows;
using Comp_v4.CompCard;
using Comp_v4.CompCard._Installers;
using Comp_v4.CompCard.Vm;
using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Entities.InputHandlers;
using Comp_v4.NomDict.Installers;
using Comp_v4.NomDict.View;
using Comp_v4.TableWindows;
using Comp_v4.TableWindows.ConditionalDesignation;
using Comp_v4.TableWindows.ConditionalDesignation.Overrided;
using Comp_v4.TableWindows.Manufacturers;
using Comp_v4.TableWindows.Manufacturers.Overrided;
using Comp_v4.TableWindows.MeasurementUnits;
using Comp_v4.TableWindows.TypeSizes;
using Comp.Db;
using Comp.ModelData.TechnicalItems;
using WPF.Services;
using WPF.Templates.TableWindow.v1.Entities.InputHandlers;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.Installers;

public partial class App : Application
{
    protected readonly AreopagContainer _rootContainer;
    protected readonly Dictionary<Type, AreopagContainer> _subContainers = new();
    
    public App() {
        _rootContainer = new AreopagContainer();
        new AppDbContextInstaller().Install(_rootContainer);

        var cont = new AreopagContainer();
        new CompCardWindowInstaller(_rootContainer, _subContainers).Install(cont);
        _subContainers[typeof(CompCardWindow)] = cont;


        _subContainers[typeof(NomDictWindow)] = new AreopagContainer();
        _subContainers[typeof(NomDictWindow)].Add<AppDbContext>().AsSingleton()
                                             .UsingFactoryMethod(() => _rootContainer.Resolve<AppDbContext>());
        _subContainers[typeof(NomDictWindow)].Add<DataGridInputHandler>()
                                             .AsScoped<NomDictWindow>()
                                             .UsingFactoryMethod(() => new DataGridInputHandler(OpenCardComponentWindow));
        var ndInst = new NomDictInstaller();
        ndInst.Install(_subContainers[typeof(NomDictWindow)]);
    }

    protected override async void OnStartup(StartupEventArgs e) {
        await _rootContainer.Resolve<DatabaseInitializer>().InitializeAsync();

        var subContainer = _subContainers[typeof(NomDictWindow)];
        var window = subContainer.BeginScope<NomDictWindow>();
        window.Closed += (_, _) => subContainer.ReleaseScope<NomDictWindow>();
        _subContainers[typeof(NomDictWindow)].Instantiate<AddCategoryAction, DeleteCategoryAction, UpdateCategoryNameAction, DataGridInputHandler>();
        window.Show();
    }

    protected override async void OnExit(ExitEventArgs e) {
        base.OnExit(e);
    }

    protected void OpenCardComponentWindow(object? parameter) {
        var container = _subContainers[typeof(CompCardWindow)];
        var window = container.BeginScope<CompCardWindow>();
        window.Closed += (_, __) => container.ReleaseScope<CompCardWindow>();
        window.Show();
    }
}