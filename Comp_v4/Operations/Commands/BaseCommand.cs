using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public abstract class BaseCommand : DeferredCommand<ModuleContext, ConditionalDesignation>
{
    protected BaseCommand(ModuleContext context) : base(context) {
        /*if (_context.DataGrid.SelectedItem is not ConditionalDesignation conditionalDesignation)
            return;
        //_item = conditionalDesignation;*/
    }

    public ConditionalDesignation Item {
        get {
            if (_context.DataGrid.SelectedItem is not ConditionalDesignation conditionalDesignation)
                throw new InvalidCastException();
            return conditionalDesignation;
        }
    }
    
    public override Task ExecuteAsync() {
        return Task.CompletedTask;
    }
    public override Task UndoAsync() {
        return Task.CompletedTask;
    }

    public override Task ExecuteDeferredAsync() {
        return Task.CompletedTask;
    }
}