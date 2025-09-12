using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberInputTextCommand : BaseCommand
{
    protected readonly DataGridPropertyRestoreService<ConditionalDesignation> _propertyRestoreService;
    
    public RememberInputTextCommand(ModuleContext context, object? parameter) : base(context, parameter) {
        _propertyRestoreService = App.Host.Services.GetRequiredService<DataGridPropertyRestoreService<ConditionalDesignation>>();
    }

    public override async Task ExecuteAsync() {
        await Task.Delay(100);
        
        if (_parameter is not DataGridBeginningEditEventArgs e)
            throw new InvalidCastException();
        
        if (e.Row.Item is not ConditionalDesignation conditionalDesignation)
            throw new InvalidCastException();
        
        _propertyRestoreService.RememberValue(conditionalDesignation, e.Column.GetPropertyName());
        _item = conditionalDesignation;
    }

    public override async Task UndoAsync() {
        _propertyRestoreService.RollbackEdit(_item!);
        // Даем время WPF обработать изменение данных
        await Task.Delay(50); 
    }
    
    
}