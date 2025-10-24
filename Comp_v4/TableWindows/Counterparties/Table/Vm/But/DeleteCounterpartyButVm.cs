using CommunityToolkit.Mvvm.Input;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.Counterparties.Table.Vm.But;

public partial class DeleteCounterpartyButVm : BaseButtonAdvanced
{
    public DeleteCounterpartyButVm() {
        Label = "Удалить";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        return base.OnClickAsync();
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}
