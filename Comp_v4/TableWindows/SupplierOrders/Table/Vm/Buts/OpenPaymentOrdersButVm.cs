using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm.Buts;

public partial class OpenPaymentOrdersButVm : BaseButtonAdvanced
{
    public OpenPaymentOrdersButVm() {
        Label = "Открыть платежные поручения";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}
