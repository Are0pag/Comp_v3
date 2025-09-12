using System.Windows.Controls;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberInputTextCommand : BaseCommand<DataGridCellEditEndingEventArgs>
{
    protected readonly DataGridPropertyRestoreService<ConditionalDesignation> _propertyRestoreService;
    public RememberInputTextCommand(DataGridCellEditEndingEventArgs parameter, 
                                    DataGridPropertyRestoreService<ConditionalDesignation> propertyRestoreService) 
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
        // Даем время WPF обработать изменение данных
        await Task.Delay(50); 
    }
    
    
}