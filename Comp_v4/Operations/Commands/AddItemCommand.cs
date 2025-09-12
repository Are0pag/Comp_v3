using Comp.Db.Contracts;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class AddItemCommand : BaseCommand
{
    protected readonly IConditionalDesignationRepository _repository;

    public AddItemCommand(ModuleContext context, object? parameter = null) : base(context, parameter) {
        _repository = App.Host.Services.GetRequiredService<IConditionalDesignationRepository>();
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.AddAsync(_item!);
    }
}