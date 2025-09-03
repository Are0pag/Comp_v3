using System.Windows.Input;
using Infrastructure.Command.Heterochromic;

namespace WPF.Services.UserActionsHandling.InputKey;

public class CommonUndoRedoHotKeysService
{
    protected readonly HeterochromicCommandScheduler<IDeferredCommand> _scheduler; /* тип должен быть другим */
    public CommonUndoRedoHotKeysService(HeterochromicCommandScheduler<IDeferredCommand> scheduler) {
        _scheduler = scheduler;
    }

    public async Task HandleInput(KeyEventArgs e) {
        switch (e.Key) {
            case Key.Z when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                if (_scheduler.CanUndo()) {
                    await _scheduler.UndoAsync();
                    e.Handled = true;
                }
                break;
            case Key.Z when e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift):
            case Key.Y when e.KeyboardDevice.Modifiers == ModifierKeys.Control:
                if (_scheduler.CanRedo()) {
                    await _scheduler.RedoAsync();
                    e.Handled = true;
                }
                break;
        }
    }
}