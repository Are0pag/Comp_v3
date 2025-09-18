using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.Operations.Commands;

public class DeleteItemCommand<T> : BaseCommand<T>
    where T : class, IDbEntity
{
    protected readonly IRepository<T> _repository;
    public DeleteItemCommand(T parameter, IRepository<T> repository) : base(parameter) {
        _repository = repository;
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.DeleteAsync(_parameter.Id);
    }
}