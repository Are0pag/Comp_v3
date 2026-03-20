using DI;
using Infrastructure.Command;
using Infrastructure.Command.Heterochromic;

namespace Comp_v4.TableWindows;

public class DataGridCommandFactory : ICommandFactory
{
    /*protected readonly AreopagContainer _container;

    public DataGridCommandFactory(AreopagContainer container) {
        _container = container;
    }*/

    public TCommand CreateCommand<TCommand, TParameter>(TParameter parameter) where TCommand : DeferredCommandBase<TParameter> {
        throw new NotImplementedException();
        /*var command = _container.Resolve<TCommand>(parameter);
        return command;*/
    }
}
