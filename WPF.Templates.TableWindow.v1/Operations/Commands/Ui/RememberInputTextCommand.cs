using System.Windows;
using System.Windows.Controls;
using Infrastructure;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;

namespace WPF.Templates.TableWindow.v1.Operations.Commands.Ui;

public class RememberInputTextCommand<TWindow, T> : BaseCommand<DataGridBeginningEditEventArgs>
    where TWindow : Window
    where T : class
{
    protected readonly IPropertyValueRestoreService<T> _propertyRestoreService;
    protected T? _item;
    
    public RememberInputTextCommand(DataGridBeginningEditEventArgs parameter, IPropertyValueRestoreService<T> propertyRestoreService) 
        : base(parameter) {
        _propertyRestoreService = propertyRestoreService;
    }

    public override async Task ExecuteAsync() {
        await Task.Delay(100);

        if (_parameter.Row.Item is not T dataType) {
            new InvalidCastException().Log(this);
            return;
        }
        
        _propertyRestoreService.RememberValue(dataType, _parameter.Column.GetPropertyName());
        _item = dataType;
    #if DEBUG
        Console.WriteLine($"Input: {_parameter.Column.GetPropertyName()}");
    #endif
    }

    public override async Task UndoAsync() {
        _propertyRestoreService.RollbackEdit(_item!);
        await Task.Delay(50); 
    }
}