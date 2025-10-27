using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests;

public class Worker : BackgroundService
{
    protected readonly IServiceScopeFactory _scopeFactory;
    
    public Worker(IServiceScopeFactory scopeFactory) {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        while (!stoppingToken.IsCancellationRequested) {
            using (IServiceScope scope = _scopeFactory.CreateScope()) {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                logger.Log("Say Hello");
            }
        }
    }
}


public interface ILogger
{
    void Log(string message);
}

public class Logger : ILogger
{
    public void Log(string message) {
        Console.WriteLine(message);
    }
}