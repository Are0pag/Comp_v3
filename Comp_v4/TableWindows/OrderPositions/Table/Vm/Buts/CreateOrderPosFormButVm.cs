using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Table.Vm.Buts;

public partial class CreateOrderPosFormButVm : BaseButtonAdvanced
{
    public CreateOrderPosFormButVm() {
        Label = "Добавить";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}