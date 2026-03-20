using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;

public partial class EditOrderPosFormButVm : BaseButtonAdvanced
{
    public EditOrderPosFormButVm() {
        Label = "Редактировать";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}