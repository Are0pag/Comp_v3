using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class AddItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly IConditionalDesignationRepository _repository;

    public AddItemCommand(ConditionalDesignation parameter) : base(parameter) {
        _repository = App.Host.Services.GetRequiredService<IConditionalDesignationRepository>();
        // TODO
        //_repository = App.TestScope.ServiceProvider.GetRequiredService<IConditionalDesignationRepository>();
        // не, надо регать как трансиент
        /*var scope = App.ProvideTransientCommandInScope<ICondDesServiceScope>();
        _repository = scope.ServiceProvider.GetRequiredService<IConditionalDesignationRepository>();*/
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.AddAsync(_parameter);
    }
}