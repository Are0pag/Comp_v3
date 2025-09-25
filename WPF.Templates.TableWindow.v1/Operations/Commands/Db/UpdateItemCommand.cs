using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.v1.Operations.Commands.Db;

public class UpdateItemCommand<T> : DeferredCommandBase<T>
    where T : class, IDbEntity
{
    protected readonly IRepository<T> _repository;
    public UpdateItemCommand(T parameter, IRepository<T> repository) : base(parameter) {
        _repository = repository;
    }

    public override async Task ExecuteDeferredAsync() {
        try {
            await _repository.UpdateAsync(_parameter);
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}