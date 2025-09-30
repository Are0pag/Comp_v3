using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Vm.Buttons;

public partial class AddNewCategoryButtonVm : BaseAsyncBButtonVm
{
    [RelayCommand(CanExecute = nameof(CanClick))]
    public override async Task OnClickAsync() {
        await ClickActionAsync?.Invoke(null);
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}
