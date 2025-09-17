using Comp_v4.Entities;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class AddItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly IRepository<ConditionalDesignation> _repository;

    public AddItemCommand(ConditionalDesignation parameter, IRepository<ConditionalDesignation> repository) : base(parameter) {
        _repository = repository;
        //_repository = App.Host.Services.GetRequiredService<IRepository<ConditionalDesignation>>();
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

public interface INewDbRepository<T> : IRepository<T> where T : class {}
public class UngradedAddItemCommand : AddItemCommand
{
    public UngradedAddItemCommand(ConditionalDesignation parameter, INewDbRepository<ConditionalDesignation> repository) : base(parameter, repository) {
    }
}