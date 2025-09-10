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

    public override Task ExecuteAsync() {
        _propertyRestoreService.RememberValue(_item!, _context.DataGrid.CurrentCell.Column.GetPropertyName());
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _propertyRestoreService.RollbackEdit(_item!);
        return Task.CompletedTask;
    }
}