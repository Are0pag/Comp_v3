using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Analogs.Buttons;

public partial class AddAnalogButtonVm : BaseButtonAdvanced
{
    public AddAnalogButtonVm() {
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