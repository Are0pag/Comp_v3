using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.VmInvoking.Request;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;

public class AddItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    public AddItemCommand(CognDesignGridVm context, ConditionalDesignation item) : base(context) {
        _item = item;
    }
    public override Task ExecuteAsync() => Task.CompletedTask;
    public override Task UndoAsync() => Task.CompletedTask;

    public override async Task ExecuteDeferredAsync() {
        await _context.Repository.AddAsync(_item);
    }
}