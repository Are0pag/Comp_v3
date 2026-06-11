using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;

public partial class CancelEditingCpButVm : BaseButtonAdvanced
{
    public CancelEditingCpButVm() {
        Label = "Отменить";
    }
    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}