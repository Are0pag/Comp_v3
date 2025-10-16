using System.Windows;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp.ModelData;

namespace Comp_v4.TableWindows.Analogs;

public partial class FormWindow : Window, IDisposable
{
    public FormWindow(Analog analog, SelectAnalogButtonVm selectAnalogButtonVm) {
        InitializeComponent();
        DataContext = analog;
        SelectAnalogButton.DataContext = selectAnalogButtonVm;
    }

    public void Dispose() {
        
    }
}