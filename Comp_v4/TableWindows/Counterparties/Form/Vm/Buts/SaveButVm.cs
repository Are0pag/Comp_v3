using CommunityToolkit.Mvvm.Input;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;

public partial class SaveButVm : BaseButtonAdvanced<Counterparty>
{
    public SaveButVm() {
        Label = "Save";
    }
    [RelayCommand]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}