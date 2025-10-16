using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.TableWindows.Analogs.Buttons;

public partial class SelectAnalogButtonVm
{
    public bool CanSelect() {
        return true;
    }

    [RelayCommand(CanExecute = nameof(CanSelect))]
    public void SelectAnalog() {
        
    }
}