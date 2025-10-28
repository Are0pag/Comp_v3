using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Templates.Common.Actions;

public abstract class BaseActionAsyncScopeHandler : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceScopeFactory _scopeFactory;
    public BaseActionAsyncScopeHandler(BaseButtonAdvanced button, IServiceScopeFactory scopeFactory) : base(button) {
        _scopeFactory = scopeFactory;
    }
    
    public IServiceScope? ParentScope { get; set; }
}