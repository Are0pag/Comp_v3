using CommunityToolkit.Mvvm.Input;
using Comp_v4.TableWindows.Analogs.Events;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Analogs.Buttons;

public partial class SelectAnalogButtonVm
{
    public bool CanSelect() {
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanSelect))]
    public void SelectAnalog() {
        EventBus<IAnalogsTableWindowSubscriber>.RaiseEvent<ISelectAnalogHandler>(h => h?.OnStartSelectingAnalog());
    }
}