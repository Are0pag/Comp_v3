using System.Windows;
using Comp_v4.CompCard;
using Comp_v4.CompCard.Vm;
using Comp_v4.Entities;
using WPF.Services;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm.Components;

using Tw = Comp_v4.TargetWindow;
using Cd = Comp.ModelData.TechnicalItems.ConditionalDesignation;

namespace Comp_v4;


public partial class App : Application
{
    protected readonly AreopagContainer _rootContainer;
    protected readonly Dictionary<Type, AreopagContainer> _subContainers = new();
    
    public App() {
        _rootContainer = new AreopagContainer();
        _rootContainer.Install();
        
    }

    protected override async void OnStartup(StartupEventArgs e) {
        new CompCardWindow(new CompCardVm(), new CdFieldVm(() => {
            _subContainers[typeof(Tw)].BeginScope<Tw>();
        })).Show();
        return;
        
        var window = _rootContainer.BeginScope<Tw>();
        window.Closed += (sender, args) => {
            _rootContainer.ReleaseScope<Tw>();
        };
        _rootContainer.Instantiate<ActionStackTracker, PersistenceManager<Tw, Cd>, TableCommandBinder<Tw, Cd>, ActionFilter<Tw, Cd, FiltersVmCd>>();
        window.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e) {
        base.OnExit(e);
    }

    protected void ToDo() {
        var installer = new CdWindowInstaller();
        var subContainer = new AreopagContainer();
        installer.Install(subContainer);
        _subContainers[typeof(Tw)] = subContainer;

        _rootContainer.Add<Tw>()
                      .UsingFactoryMethod(() => {
                           return _subContainers[typeof(Tw)].BeginScope<Tw>();
                       });
    }
}

public class ProgWindow {}
public class CompWindow {}