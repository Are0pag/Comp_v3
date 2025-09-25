using System.Windows;
using Comp_v4.CompCard;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows.ConditionalDesignation.Installers;
using WPF.Services;
using WPF.Templates.TableWindow.v1.Entities.InputHandlers;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm.Components;
using Tw = Comp_v4.TableWindows.ConditionalDesignation.TargetWindow;
using Cd = Comp.ModelData.TechnicalItems.ConditionalDesignation;

namespace Comp_v4.Installers;

public partial class App : Application
{
    protected readonly AreopagContainer _rootContainer;
    protected readonly Dictionary<Type, AreopagContainer> _subContainers = new();
    
    public App() {
        _rootContainer = new AreopagContainer();
        var cdWindowInstaller = new TableWindowInstaller<Tw, Cd>();
        
        var subContainer = new AreopagContainer();
        cdWindowInstaller.Install(subContainer);

        new AppDbContextInstaller().Install(subContainer);

        _subContainers[typeof(Tw)] = subContainer;
    }

    protected override async void OnStartup(StartupEventArgs e) {
        new CompCardWindow(new CompCardVm(), new CdFieldVm(() => {
            var contextContainer = _subContainers[typeof(Tw)];
            var window = contextContainer.BeginScope<Tw>();
            window.Closed += (sender, args) => {
                contextContainer.ReleaseScope<Tw>();
            };
            contextContainer.Instantiate<ActionStackTracker, PersistenceManager<Tw, Cd>, TableCommandBinder<Tw, Cd>, ActionFilter<Tw, Cd, FiltersVmBase>>();
            window.Show();
        })).Show();
    }

    protected override async void OnExit(ExitEventArgs e) {
        base.OnExit(e);
    }
}