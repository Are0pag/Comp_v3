using CommunityToolkit.Mvvm.Input;
using WPF.Templates.Core;

namespace WPF.Templates;

public partial class ButtonVmAddItem : BaseButtonsVm<ActionAddItem>
{
    public ButtonVmAddItem(ActionAddItem context) : base(context) {
    }

    [RelayCommand]
    protected async Task AddItem() => await _context.PerformAsync();
    protected bool CanAddItem() => _context.CanPerform();
    public override void NotifyCanExecute() => AddItemCommand.NotifyCanExecuteChanged();
}