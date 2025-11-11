using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.CompCard.Vm.Buttons;

public partial class AnalogsFieldButtonVm : BaseButtonAdvanced
{
    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }
}