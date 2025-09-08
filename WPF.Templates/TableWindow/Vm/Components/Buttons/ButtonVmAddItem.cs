using CommunityToolkit.Mvvm.Input;
using WPF.Templates.Core;

namespace WPF.Templates;

public partial class ButtonVmAddItem : BaseButtonsVm<GridContentEditor>
{
    public ButtonVmAddItem(GridContentEditor context) : base(context) {
    }

    [RelayCommand]
    protected async Task AddItem() => await _context.AddItem.PerformAsync();

    protected bool CanAddItem() => _context.AddItem.CanPerform();

    public override void NotifyCanExecute() => AddItemCommand.NotifyCanExecuteChanged();
}