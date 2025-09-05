using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Window;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.VmInvoking.Request;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;

public class AddItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>, IDataGridRequester
{
    protected readonly CursorPositionService<System.Windows.Controls.DataGrid> _dataGridCursorPositionService;
    protected DataGridMemento? _dataGridMemento;
    public AddItemCommand(CognDesignGridVm context, ConditionalDesignation item) : base(context) {
        _item = item;
        _dataGridCursorPositionService = App.Host.Services.GetRequiredService<CursorPositionService<System.Windows.Controls.DataGrid>>();
    }

    public System.Windows.Controls.DataGrid DataGrid { get; set; }

    public override Task ExecuteAsync() {
        EventBus<IVmGlobalSubscriber>.RaiseEvent<IDataGridRequestResolver>(r => r!.GetGrid(this));
        _dataGridMemento = _dataGridCursorPositionService.RememberCursorPos(DataGrid);
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _dataGridMemento!.Restore(DataGrid);
        return Task.CompletedTask;
    }

    public override async Task ExecuteDeferredAsync() {
        await _context.Repository.AddAsync(_item);
    }
}