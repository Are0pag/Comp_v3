using System.Windows.Controls;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.VmInvoking.Request;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class UpdateItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>, IDataGridRequester
{
    protected readonly CursorPositionService<System.Windows.Controls.DataGrid> _cursorPositionService;
    protected readonly DataGridCellEditEndingEventArgs _e;
    protected CellMemento? _gridMementoOnEditing;

    public UpdateItemCommand(CognDesignGridVm context, DataGridCellEditEndingEventArgs e) : base(context) {
        _e = e;
        _cursorPositionService = App.Host.Services.GetRequiredService<CursorPositionService<System.Windows.Controls.DataGrid>>();
    }

    public System.Windows.Controls.DataGrid DataGrid { get; set; }

    public override Task ExecuteAsync() {
        EventBus<IVmGlobalSubscriber>.RaiseEvent<IDataGridRequestResolver>(r => r.GetGrid(this));
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _cursorPositionService.FocusAndEditItem(DataGrid, _e);
        return Task.CompletedTask;
    }

    public override async Task ExecuteDeferredAsync() {
        await _context.Repository.UpdateAsync(_item);
    }
}