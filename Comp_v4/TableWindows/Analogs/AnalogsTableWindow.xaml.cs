using System.Windows;
using System.Windows.Input;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs;

public partial class AnalogsTableWindow : Window, IDisposable
{
    protected readonly EditAnalogButVm _editAnalogButVm;
    public AnalogsTableWindow(AnalogsTableVm analogsTableVm, AddAnalogButtonVm addAnalogButtonVm, EditAnalogButVm editAnalogButVm) {
        InitializeComponent();
        _editAnalogButVm = editAnalogButVm;
        MainDataGrid.DataContext = analogsTableVm;
        AddAnalogButton.DataContext = addAnalogButtonVm;
    }

    public void Dispose() {
        
    }

    private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        //EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<IMouseDoubleClickHandler>(h => h.OnMouseDoubleClick(sender, e));
        _editAnalogButVm.OnClickAsync();
    }
}