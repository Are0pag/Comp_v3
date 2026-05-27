using System.ComponentModel;
using System.Windows;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs;

public partial class AnalogsFormWindow : Window, IDisposable, IAnalogSaveHandler, IRuntimeParamsResolver<Analog>, IRuntimeParamsResolver<AnalogsFormWindow>
{
    protected readonly Analog _analog;
    public AnalogsFormWindow(Analog analog, SelectAnalogButtonVm selectAnalogButtonVm, SaveAnalogButVm saveButVm) {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
        _analog = analog;
        DataContext = analog;
        SelectAnalogButton.DataContext = selectAnalogButtonVm;
        SaveButton.DataContext = saveButVm;
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
        EventBus<IGlSubscriber>.Unsubscribe(this);
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
    }

    public Task Save(TaskCompletionSource tcs, Analog analog) {
        Close();
        tcs.SetResult();
        return Task.CompletedTask;
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<Analog> container) {
        container.RuntimeParam = _analog;
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<AnalogsFormWindow> container) {
        container.RuntimeParam = this;
    }
    
    private void SavePlacement(object? s, CancelEventArgs e) => WindowSettings.SavePlacement(this, GetType().ToString());
    private void LoadPlacement(object? s, EventArgs e) => WindowSettings.LoadPlacement(this, GetType().ToString());
}