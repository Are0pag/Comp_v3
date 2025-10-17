using System.Windows;
using Comp_v4.TableWindows.Analogs.Buttons;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs;

public partial class FormWindow : Window, IDisposable, ISaveHandler
{
    public FormWindow(Analog analog, SelectAnalogButtonVm selectAnalogButtonVm, SaveButVm saveButVm) {
        InitializeComponent();
        DataContext = analog;
        SelectAnalogButton.DataContext = selectAnalogButtonVm;
        SaveButton.DataContext = saveButVm;
        EventBus<IAnalogsTableWindowSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IAnalogsTableWindowSubscriber>.Unsubscribe(this);
    }

    public Task Save(TaskCompletionSource tcs) {
        Close();
        tcs.SetResult();
        return Task.CompletedTask;
    }
}