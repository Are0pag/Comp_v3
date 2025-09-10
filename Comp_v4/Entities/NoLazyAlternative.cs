using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.Entities;

public class NoLazyAlternative
{
    protected readonly ActionStackTracker _stackTracker;

    public NoLazyAlternative(ActionStackTracker stackTracker) {
        _stackTracker = new ActionStackTracker(App.Host.Services.GetRequiredService<IModuleCommandScheduler>());
    }
}