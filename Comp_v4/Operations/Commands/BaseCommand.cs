using System.Reflection.Metadata;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public abstract class BaseCommand<TParameter> : DeferredCommand<ConditionalDesignation>
{
    protected readonly TParameter _parameter;
    protected BaseCommand(TParameter parameter) {
        _parameter = parameter;
    }
    
    public override Task ExecuteAsync() => Task.CompletedTask;
    public override Task UndoAsync() => Task.CompletedTask;
    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}