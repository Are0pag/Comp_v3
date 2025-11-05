using System.Windows;
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
        //Console.WriteLine("1. Perform started");
        _currentTcs = tcs;

        _currentScope = _scopeFactory.CreateScope();

        if (GetWindow() is not ({ } window and IReloadable reloadableWindow))
            throw new Exception();
        
        window.Closed += OnWindowClosed;
        
        reloadableWindow.OnReload += async () => {
            //Console.WriteLine("3. OnReload started");
            window.Close();
            
            await Task.Delay(1);
            //Console.WriteLine("6. After window.Close()");
            
            await _button.OnClickAsync();
            //Console.WriteLine("7. OnReload completed");
        };
        
        InstantiateRelatedServices();
            
        window.Show();
        //Console.WriteLine("2. Before await tcs.Task");
        
        await tcs.Task;
        //Console.WriteLine("5.1 After await tcs.Task - Perform completed");
    }
}