using System.Windows;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs;

public partial class AnalogsFormWindow : Window, IDisposable, ISaveHandler, IRuntimeParamsResolver<Analog>
{
    protected readonly Analog _analog;
    public AnalogsFormWindow(Analog analog, SelectAnalogButtonVm selectAnalogButtonVm, SaveAnalogButVm saveButVm) {
        InitializeComponent();
        _analog = analog;
        DataContext = analog;
        SelectAnalogButton.DataContext = selectAnalogButtonVm;
        SaveButton.DataContext = saveButVm;
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<Analog> container) {
        container.RuntimeParam = _analog;
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public Task Save(TaskCompletionSource tcs, Analog analog) {
        Close();
        tcs.SetResult();
        return Task.CompletedTask;
    }
}