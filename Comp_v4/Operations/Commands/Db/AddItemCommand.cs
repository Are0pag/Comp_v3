using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.Operations.Commands;

public class AddItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly IRepository<ConditionalDesignation> _repository;

    public AddItemCommand(ConditionalDesignation parameter, IRepository<ConditionalDesignation> repository) : base(parameter) {
        _repository = repository;
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.AddAsync(_parameter);
    }
}