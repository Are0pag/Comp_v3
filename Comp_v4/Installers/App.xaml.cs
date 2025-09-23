using System.Windows;
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
    
    public App() {
        _rootContainer = new AreopagContainer();
        _rootContainer.Install();
    }

    protected override async void OnStartup(StartupEventArgs e) {

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
}