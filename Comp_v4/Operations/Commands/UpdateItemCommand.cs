using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class UpdateItemCommand : BaseCommand
{
    protected readonly IConditionalDesignationRepository _repository;
    
    public UpdateItemCommand(ModuleContext context) : base(context) {
        _repository = App.Host.Services.GetRequiredService<IConditionalDesignationRepository>();
    }

    public override async Task ExecuteDeferredAsync() {
        await _repository.UpdateAsync(_item!);
    }
}