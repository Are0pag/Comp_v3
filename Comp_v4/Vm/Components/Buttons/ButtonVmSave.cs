using CommunityToolkit.Mvvm.Input;
using Comp_v4.Operations.Commands;
using WPF.Templates.Core;

namespace WPF.Templates;

public partial class ButtonVmSave : BaseButtonsVm<ActionSave>
{
    public ButtonVmSave(ActionSave context) : base(context) {
        
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    protected async Task Save() => await _context.PerformAsync();
    protected bool CanSave() => _context.CanPerform();
    public override void NotifyCanExecute() {
        SaveCommand.NotifyCanExecuteChanged();
    }
}