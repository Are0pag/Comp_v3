using System.Windows;
using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using Infrastructure;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;

namespace Comp_v4.Operations.Commands;

public class RememberInputTextCommand<TWindow, T> : BaseCommand<DataGridBeginningEditEventArgs>
    where TWindow : Window
    where T : class
{
    protected readonly IPropertyValueRestoreService<ConditionalDesignation> _propertyRestoreService;
    protected ConditionalDesignation? _item;
    
    public RememberInputTextCommand(DataGridBeginningEditEventArgs parameter, IPropertyValueRestoreService<ConditionalDesignation> propertyRestoreService) 
        : base(parameter) {
        _propertyRestoreService = propertyRestoreService;
    }

    public override async Task ExecuteAsync() {
        await Task.Delay(100);

        if (_parameter.Row.Item is not ConditionalDesignation conditionalDesignation) {
            new InvalidCastException().Log(this);
            return;
        }
        
        _propertyRestoreService.RememberValue(conditionalDesignation, _parameter.Column.GetPropertyName());
        _item = conditionalDesignation;
    }

    public override async Task UndoAsync() {
        _propertyRestoreService.RollbackEdit(_item!);
        await Task.Delay(50); 
    }
}