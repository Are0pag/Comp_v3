using Comp.Db.Contracts;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class AddItemCommand : BaseCommand<object>
{
    protected readonly IConditionalDesignationRepository _repository;

    public AddItemCommand(object? parameter, IConditionalDesignationRepository repository) : base(parameter) {
        _repository = repository;
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.AddAsync(_item!);
    }
}