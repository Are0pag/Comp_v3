using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;

public partial class EditPaymentOrderButVm : BaseButtonAdvanced
{
    public EditPaymentOrderButVm() {
        Label = "Редактировать платежное поручение";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}
