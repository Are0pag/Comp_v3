using CommunityToolkit.Mvvm.Input;
using Comp_v4.TableWindows.Analogs.Events;
using Comp.ModelData.Comp;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Buttons;

public partial class SelectAnalogButtonVm
{
    protected bool _isEnabled = true;
    public bool CanSelect() {
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanSelect))]
    protected async Task SelectAnalog() {
        if (!_isEnabled)
            return;
        
        _isEnabled = false;
        var tsc = new TaskCompletionSource<Component>();
        EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<ISelectAnalogHandler>(h => h?.OnStartSelectingAnalog(tsc));
        
        await tsc.Task;
        _isEnabled = true;
    }
}