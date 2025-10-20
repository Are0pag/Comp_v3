using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.Entry.Vm.Buts;

public partial class OrdersButVm : BaseButtonAdvanced
{
    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand?.NotifyCanExecuteChanged();
    }
}