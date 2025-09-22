using System.Windows;
using Castle.Windsor;
using Tests.Castle.Entities.Ui;
using WPF.Services;

namespace Tests;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected readonly Container _myRootContainer;

    public App() {
        _myRootContainer = new Container();
        _myRootContainer.Install();
    }
    
    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);

        var catMom = _myRootContainer.BeginScope<ICatMom>();
        var dogInt = _myRootContainer.Resolve<DogInt>(144008, "Dog in fear dress OПГ Солнышко");
        _myRootContainer.ReleaseScope<ICatMom>();

        var sc = _myRootContainer.Resolve<SmokeCat>();

        //new ReferenceTracker().TrackObjectReferences(catMom);
        /*var window = _myRootContainer.Resolve<FloatingWindow>();
        window.Show();*/
    }
}