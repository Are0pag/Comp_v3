using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using WPF.Templates;
using WPF.Templates.TableWindow.Events;

namespace Comp_v4.Operations.Commands;

public class UpdateItemCommand : BaseCommand
{
    protected readonly IConditionalDesignationRepository _repository;
    
    public UpdateItemCommand(ModuleContext context) : base(context) {
        _repository = App.Host.Services.GetRequiredService<IConditionalDesignationRepository>();
        _item = Item;
    }

    public override async Task ExecuteDeferredAsync() {
        try {
            await _repository.UpdateAsync(_item!);
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }
}