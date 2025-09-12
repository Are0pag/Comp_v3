using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public abstract class BaseCommand<TParameter> : DeferredCommand<ConditionalDesignation>
{
    protected readonly TParameter _parameter;
    protected readonly ModuleContext _moduleContext;
    protected BaseCommand(TParameter parameter) {
        _parameter = parameter;
        _moduleContext = App.Host.Services.GetRequiredService<ModuleContext>();
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