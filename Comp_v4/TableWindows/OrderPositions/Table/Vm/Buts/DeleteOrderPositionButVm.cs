using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;

public partial class DeleteOrderPositionButVm : BaseButtonAdvanced
{
    public DeleteOrderPositionButVm() {
        Label = "Удалить позицию заказа";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}