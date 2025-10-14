using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.TableWindows.TypeSizes.Vm.Buttons;

public partial class ButtonSaveNewItemForm
{
    protected readonly ActionSaveNewItemForm _context;
    public ButtonSaveNewItemForm(ActionSaveNewItemForm context) {
        _context = context;
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    public async Task Save() => _context.Perform();
    public bool CanSave() => true;
    
    public void NotifyCanExecute() {
        SaveCommand.NotifyCanExecuteChanged();
    }
}