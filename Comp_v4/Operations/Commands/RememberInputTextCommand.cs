using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberInputTextCommand : BaseCommand
{
    protected readonly DataGridPropertyRestoreService<ConditionalDesignation> _propertyRestoreService;
    
    public RememberInputTextCommand(ModuleContext context) : base(context) {
        _propertyRestoreService = App.Host.Services.GetRequiredService<DataGridPropertyRestoreService<ConditionalDesignation>>();
    }

    public override async Task ExecuteAsync() {
        await Task.Delay(100);
        _propertyRestoreService.RememberValue(_item!, _context.DataGrid.CurrentCell.Column.GetPropertyName());
    }

    public override async Task UndoAsync() {
        _propertyRestoreService.RollbackEdit(_item!);
        // Даем время WPF обработать изменение данных
        await Task.Delay(50); 
    }
}