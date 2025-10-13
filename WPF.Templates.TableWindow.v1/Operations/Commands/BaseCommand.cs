using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.v1.Operations.Commands;

public abstract class BaseCommand<TParameter> : DeferredCommandBase<TParameter>
    where TParameter : class
{
    protected BaseCommand(TParameter parameter) : base(parameter) {
    }

    public override Task ExecuteAsync() => Task.CompletedTask;
    public override Task UndoAsync() => Task.CompletedTask;
    public override Task ExecuteDeferredAsync() => Task.CompletedTask;

    public override string ToString() {
        return this.GetType().Name;
    }
}