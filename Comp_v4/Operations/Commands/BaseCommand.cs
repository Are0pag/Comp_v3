using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public abstract class BaseCommand : DeferredCommand<ModuleContext, ConditionalDesignation>
{
    protected BaseCommand(ModuleContext context) : base(context) {
        if (_context.DataGrid.SelectedItem is not ConditionalDesignation conditionalDesignation)
            return;
        _item = conditionalDesignation;
    }
    
    
}