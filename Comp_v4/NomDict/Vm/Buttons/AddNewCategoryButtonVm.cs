using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.Vm.Buttons;

public partial class AddNewCategoryButtonVm : BaseButtonVm
{
    public AddNewCategoryButtonVm(Func<bool> canExecuteFunc) : base(canExecuteFunc) {
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override void OnClick() {
        ClickAction?.Invoke(null);
    }

    public override void NotifyCanExecute() {
        
    }
}