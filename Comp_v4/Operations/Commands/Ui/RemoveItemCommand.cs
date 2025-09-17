using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Templates;
using WPF.Templates.TableWindow.Events;

namespace Comp_v4.Operations.Commands;

public class RemoveItemCommand : BaseCommand<ConditionalDesignation>
{
    protected readonly ModuleContext _context;
    public RemoveItemCommand(ConditionalDesignation parameter, ModuleContext context) : base(parameter) {
        _context = context;
    }

    public override Task ExecuteAsync() {
        _context.DataGridViewModel.Items.Remove(_parameter);
        return Task.CompletedTask;
    }

    public override async Task UndoAsync() {
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringHandler>(h => h?.OnSourceCollectionStartEditing());
        await Task.Delay(100);
        var collection = _context.DataGridViewModel.Items;
        
        collection.Add(_parameter);
        
        _context.DataGrid.ScrollIntoView(_parameter);
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringHandler>(h => h?.OnSourceCollectionStopEditing());
    }
}