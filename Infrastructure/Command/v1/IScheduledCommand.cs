using Infrastructure.Command.Base;

namespace Infrastructure.Command.v1;

public interface IScheduledCommand : ICommand
{
    DateTime ScheduledTime { get; }
    int Priority { get; }
    bool CanExecute();
}