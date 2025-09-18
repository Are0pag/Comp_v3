using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace Comp_v4.Operations.Commands;

public abstract class BaseCommand<TParameter> : DeferredCommandBase<TParameter>
    where TParameter : class
{
    protected BaseCommand(TParameter parameter) : base(parameter) {
    }

    public override Task ExecuteAsync() => Task.CompletedTask;
    public override Task UndoAsync() => Task.CompletedTask;
    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}