using System.Windows;
using Comp_v3.Test;

namespace Comp_v3;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e) {
        base.OnStartup(e);
        new Window1().Show();
    }
}