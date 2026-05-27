using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs;

public partial class AnalogsTableWindow : Window, IDisposable, IRuntimeParamsResolver<AnalogsTableWindow>
{
    protected readonly EditAnalogButVm _editAnalogButVm;
    public AnalogsTableWindow(AnalogsTableVm analogsTableVm, AddAnalogButtonVm addAnalogButtonVm, EditAnalogButVm editAnalogButVm) {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
        _editAnalogButVm = editAnalogButVm;
        MainDataGrid.DataContext = analogsTableVm;
        AddAnalogButton.DataContext = addAnalogButtonVm;
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<AnalogsTableWindow> container) {
        container.RuntimeParam = this;
    }

    private void SavePlacement(object? s, CancelEventArgs e) => WindowSettings.SavePlacement(this, GetType().ToString());
    private void LoadPlacement(object? s, EventArgs e) => WindowSettings.LoadPlacement(this, GetType().ToString());
    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
    }

    private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        //EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<IMouseDoubleClickHandler>(h => h.OnMouseDoubleClick(sender, e));
        //_editAnalogButVm.OnClickAsync(); Не делаю, нахуй вообще нужно
    }
}