using Infrastructure.Command.Classic;

namespace Infrastructure.Command;

public interface ICommandInterface : ICommand
{
    public abstract Task ExecuteFrontPartAsync();
}