using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.TableWindows.TypeSizes.Vm.Buttons;

public partial class ButtonOpenItemForm
{
    protected readonly ActionOpenTsForm _context;
    public ButtonOpenItemForm(ActionOpenTsForm context) {
        _context = context;
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    public async Task Open() => _context.PerformAsync();
    public bool CanSave() => true;
    
    public void NotifyCanExecute() {
        OpenCommand.NotifyCanExecuteChanged();
    }
}