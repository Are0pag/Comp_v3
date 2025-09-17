using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using WPF.Templates;
using WPF.Templates.TableWindow.Events;

namespace Comp_v4.Operations.Commands;

public class UpdateItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly IRepository<ConditionalDesignation> _repository;
    public UpdateItemCommand(ConditionalDesignation parameter) : base(parameter) {
        _repository = App.Host.Services.GetRequiredService<IRepository<ConditionalDesignation>>();
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