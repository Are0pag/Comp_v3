using CommunityToolkit.Mvvm.Input;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.TypeSizes.Vm.Buttons;

public partial class ButtonSaveNewItemForm : INotifyConditionalsChanged
{
    protected readonly ActionSaveNewTsForm _context;
    public ButtonSaveNewItemForm(ActionSaveNewTsForm context) {
        _context = context;
        EventBus<IGlobalButtonEvent>.Subscribe(this);
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    public async Task Save() => _context.Perform();

    public bool CanSave() => _context.CanPerform();
    
    public void NotifyCanExecute() {
        SaveCommand.NotifyCanExecuteChanged();
    }

    public void Dispose() {
        EventBus<IGlobalButtonEvent>.Unsubscribe(this);
    }
}