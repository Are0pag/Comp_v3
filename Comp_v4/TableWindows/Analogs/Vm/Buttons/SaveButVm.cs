using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Buttons;

public partial class SaveButVm : BaseButtonAdvanced
{
    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}