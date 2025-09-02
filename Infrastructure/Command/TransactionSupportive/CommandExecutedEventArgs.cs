using Infrastructure.Command.Classic;

namespace Infrastructure.Command;

public class CommandExecutedEventArgs : EventArgs
{
    public ICommand Command { get; }
    public CommandAction Action { get; }
    
    public CommandExecutedEventArgs(ICommand command, CommandAction action) {
        Command = command;
        Action = action;
    }
}