using System.Windows;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Tests.Castle.Entities.Ui;

namespace Tests;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected readonly IWindsorContainer _rootContainer;

    public App() {
        _rootContainer = new WindsorContainer();
        _rootContainer.Install(FromAssembly.This());
    }
    
    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);
        
        //var scope = _rootContainer.BeginScope();
        var vm = _rootContainer.Resolve<DogInt>(new Arguments() {
            {"value", 33308}
        });
        //var window = _rootContainer.Resolve<FloatingWindow>();
        //window.Closed += (o, args) => scope.Dispose();
        //window.Show();
    }
}