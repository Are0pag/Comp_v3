using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class AddItemCommand : BaseCommand<object>
{
    protected readonly IConditionalDesignationRepository _repository;

    public AddItemCommand(object parameter) : base(parameter) {
        _repository = App.Host.Services.GetRequiredService<IConditionalDesignationRepository>();
    }

    public override async Task ExecuteDeferredAsync() {
        if (_parameter is not ConditionalDesignation conditionalDesignation) {
            new InvalidOperationException("Parameter is not a conditional designation").Log(this);
            return;
        }
        await _repository.AddAsync(conditionalDesignation);
    }
}