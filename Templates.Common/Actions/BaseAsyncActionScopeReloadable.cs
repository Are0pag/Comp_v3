using System.Windows;
using Comp_v4;
using Microsoft.Extensions.DependencyInjection;
using Utils.WPF.Buttons;

namespace Templates.Common.Actions;

public abstract class BaseAsyncActionScopeReloadable : BaseActionAsyncScopeHandler
{
    protected BaseAsyncActionScopeReloadable(BaseButtonAdvanced button, IServiceScopeFactory scopeFactory) : base(button, scopeFactory) {
    }

    protected abstract Window GetWindow();

    protected abstract void InstantiateRelatedServices();
    
    public override async Task Perform(TaskCompletionSource tcs) {
        if (AppConfig.IS_LOG_RELOAD_SCOPE) Console.WriteLine("1. Perform started");
        _currentTcs = tcs;

        _currentScope = _scopeFactory.CreateScope();

        if (GetWindow() is not ({ } window and IReloadable reloadableWindow))
            throw new Exception();
        
        window.Closed += OnWindowClosed;
        
        reloadableWindow.OnReload += async () => {
            if (AppConfig.IS_LOG_RELOAD_SCOPE) Console.WriteLine("3. OnReload started");
            window.Close();
            
            await Task.Delay(AppConfig.TCS_EXECUTION_DELAY);
            if (AppConfig.IS_LOG_RELOAD_SCOPE) Console.WriteLine("6. After window.Close()");
            
            _button.OnClickAsync();
            if (AppConfig.IS_LOG_RELOAD_SCOPE) Console.WriteLine("7. OnReload completed");
        };
        
        InstantiateRelatedServices();
            
        window.Show();
        if (AppConfig.IS_LOG_RELOAD_SCOPE) Console.WriteLine("2. Before await tcs.Task");
        
        await tcs.Task;
        if (AppConfig.IS_LOG_RELOAD_SCOPE) Console.WriteLine("5.1 After await tcs.Task - Perform completed");
    }
}