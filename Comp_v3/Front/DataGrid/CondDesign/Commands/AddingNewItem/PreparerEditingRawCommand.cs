using Comp_v3.Front.DataGrid.CondDesign.Window;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.VmInvoking.Request;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;

public class PreparerEditingRawCommand : DeferredCommandBase<>, IDataGridRequester<CognDesignGridWindow>
{
    protected readonly CursorPositionService<System.Windows.Controls.DataGrid> _dataGridCursorPositionService;
    protected DataGridMemento? _dataGridMemento;
    
    public PreparerEditingRawCommand(CognDesignGridWindow? context) : base(context) {
        _dataGridCursorPositionService = App.Host.Services.GetRequiredService<CursorPositionService<System.Windows.Controls.DataGrid>>();
        DataGrid = context?.InfoDataGrid;
    }

    public System.Windows.Controls.DataGrid DataGrid { get; set; }

    public override Task ExecuteAsync() {
        if (DataGrid == null)
            EventBus<IVmGlobalSubscriber>.RaiseEvent<IDataGridRequestResolver<CognDesignGridWindow>>(r => r!.GetGrid(this));
        //_dataGridMemento = _dataGridCursorPositionService.RememberCursorPos(DataGrid!);
        /* В общем эту команду автоматически выполняет WPF */
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _dataGridCursorPositionService.MoveCursorToLeftCell(DataGrid);
        return Task.CompletedTask;
    }

    public override Task ExecuteDeferredAsync() {
        return Task.CompletedTask;
    }
}