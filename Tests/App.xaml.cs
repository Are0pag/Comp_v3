using System.Windows;
using Castle.Windsor;
using Castle.Windsor.Installer;

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
    }
}