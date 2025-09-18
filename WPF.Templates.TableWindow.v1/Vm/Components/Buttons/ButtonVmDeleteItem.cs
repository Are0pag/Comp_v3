using CommunityToolkit.Mvvm.Input;
using WPF.Templates.Core;

namespace WPF.Templates;

public partial class ButtonVmDeleteItem : BaseButtonsVm<ActionDeleteItem>
{
    public ButtonVmDeleteItem(ActionDeleteItem context) : base(context) {
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    protected async Task Delete() => await _context.PerformAsync();
    protected bool CanDelete() => _context.CanPerform();
    public override void NotifyCanExecute() {
        DeleteCommand.NotifyCanExecuteChanged();
    }
}