using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;

public partial class SavePaymentOrderButVm : BaseButtonAdvanced
{
    public SavePaymentOrderButVm() {
        Label = "Сохранить платежное поручение";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}
