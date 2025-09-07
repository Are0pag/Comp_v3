namespace Infrastructure.Command.v1;

public class CommandScheduler {
    private readonly PriorityQueue<IScheduledCommand, (DateTime, int)> _queue = new(new ScheduledCommandComparer());
    private readonly CancellationTokenSource _cts = new();
    private Task _processingTask;

    public CommandScheduler() {
        StartProcessing();
    }

    public void ScheduleCommand(IScheduledCommand command) {
        lock (_queue) {
            _queue.Enqueue(command, (command.ScheduledTime, command.Priority));
        }
    }

    public IEnumerable<IScheduledCommand> GetScheduledCommands() {
        lock (_queue) {
            return _queue.UnorderedItems.Select(x => x.Element).ToList();
        }
    }

    private void StartProcessing() {
        _processingTask = Task.Run(async () => {
            while (!_cts.Token.IsCancellationRequested) {
                IScheduledCommand nextCommand = null;
                
                lock (_queue) {
                    if (_queue.TryPeek(out var command, out var priority) && 
                        DateTime.Now >= command.ScheduledTime && 
                        command.CanExecute()) {
                        nextCommand = _queue.Dequeue();
                    }
                }

                if (nextCommand != null) {
                    try {
                        await nextCommand.ExecuteAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex) {
                        // Логирование ошибок
                        Console.WriteLine($"Command execution failed: {ex.Message}");
                    }
                }
                else {
                    await Task.Delay(100, _cts.Token).ConfigureAwait(false);
                }
            }
        }, _cts.Token);
    }

    public async Task StopAsync() {
        _cts.Cancel();
        await _processingTask.ConfigureAwait(false);
    }
}