using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public abstract class BaseCommand : DeferredCommand<ModuleContext, ConditionalDesignation>
{
    protected readonly object? _parameter;
    
    protected BaseCommand(ModuleContext context) : base(context) { }

    protected BaseCommand(ModuleContext context, object? parameter) : base(context) {
        _parameter = parameter;
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