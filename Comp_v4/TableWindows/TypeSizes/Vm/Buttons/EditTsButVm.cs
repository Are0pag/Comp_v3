using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.TypeSizes.Vm.Buttons;

public partial class EditTsButVm : BaseButtonAdvanced
{
    public EditTsButVm() {
        Label = "Редактировать";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override async Task OnClickAsync() {
        await base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}