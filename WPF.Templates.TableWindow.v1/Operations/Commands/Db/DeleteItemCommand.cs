using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.Operations.Commands;

public class DeleteItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly IRepository<ConditionalDesignation> _repository;
    public DeleteItemCommand(ConditionalDesignation parameter, IRepository<ConditionalDesignation> repository) : base(parameter) {
        _repository = repository;
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.DeleteAsync(_parameter.Id);
    }
}