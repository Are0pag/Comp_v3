using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;

public partial class CancelEditingOpButVm : BaseButtonAdvanced
{
    public CancelEditingOpButVm() {
        Label = "Отмена";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}