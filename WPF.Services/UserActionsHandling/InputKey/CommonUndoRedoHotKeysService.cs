using System.Windows.Input;
using Infrastructure.Command.Heterochromic;

namespace WPF.Services.UserActionsHandling.InputKey;

public enum ActionType
{
    None,
    Undo,
    Redo,
    Cancel,
}

public class CommonUndoRedoHotKeysService
{
    public ActionType HandleInput(KeyEventArgs e) {
        switch (e.Key) {
            case Key.Z when e.KeyboardDevice.Modifiers == ModifierKeys.Control: 
                return ActionType.Undo;
            
            case Key.Z when e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift):
            case Key.Y when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                return ActionType.Redo;
            
            
            case Key.Escape:
                return ActionType.Cancel;
        }
        return ActionType.None;
    }
}