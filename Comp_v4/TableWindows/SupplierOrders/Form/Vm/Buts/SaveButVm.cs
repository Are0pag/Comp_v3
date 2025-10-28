using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;

public partial class SaveButVm : BaseButtonAdvanced
{
    public SaveButVm() {
        Label = "Сохранить";
    }
    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }

    public override void Dispose() {
        base.Dispose();
        Console.WriteLine($"{nameof(SaveButVm)} disposed");
    }
}