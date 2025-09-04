using System.Windows.Input;
using Infrastructure.Command.Heterochromic;

namespace WPF.Services.UserActionsHandling.InputKey;

public enum ActionType
{
    None,
    Undo,
    Redo
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
        }
        return ActionType.None;
    }
}