using Infrastructure.Command.Base;

namespace WPF.Services.UserActionsHandling.InputKey;

public class CommonUndoRedoHotKeysService<TCommand, TScheduler> 
    where TScheduler : CommandScheduler<TCommand> 
    where TCommand : ICommand
{
    
}