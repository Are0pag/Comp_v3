using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.Operations.Commands;

public class AddItemCommand<T> : BaseCommand<T>
    where T : class, IDbEntity
{
    protected readonly IRepository<T> _repository;

    public AddItemCommand(T parameter, IRepository<T> repository) : base(parameter) {
        _repository = repository;
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.AddAsync(_parameter);
    }
}