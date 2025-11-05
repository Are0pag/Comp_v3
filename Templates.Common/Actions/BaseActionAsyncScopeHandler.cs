using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Templates.Common.Actions;

public abstract class BaseActionAsyncScopeHandler : BaseActionAsyncSelfWaiting
{
    protected readonly IServiceScopeFactory _scopeFactory;
    protected IServiceScope? _currentScope;
    
    public BaseActionAsyncScopeHandler(BaseButtonAdvanced button, IServiceScopeFactory scopeFactory) : base(button) {
        _scopeFactory = scopeFactory;
    }
    
    public IServiceScope? ParentScope { get; set; }
    
    protected void OnWindowClosed(object? sender, EventArgs e) {
        //Console.WriteLine("4.1. OnWindowClosed started");
        if (sender is Window window) {
            window.Closed -= OnWindowClosed;
        }
        Cleanup();
        //Console.WriteLine("4.2. OnWindowClosed completed");
    }

    protected void Cleanup() {
        _currentTcs?.TrySetResult();
        _currentScope?.Dispose();
        _currentScope = null;
        _currentTcs = null;
    }
}