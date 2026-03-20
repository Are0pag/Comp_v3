using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Vm.Buttons.Components;

public partial class EditCompButVm : BaseButtonAdvanced
{
    public EditCompButVm() {
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