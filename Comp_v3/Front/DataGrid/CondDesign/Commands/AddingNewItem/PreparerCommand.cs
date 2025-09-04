using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Window;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.VmInvoking.Request;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Utils.EventBus;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands.AddingNewItem;

public class PreparerCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    public PreparerCommand(CognDesignGridVm context) : base(context) {
    }

    public ConditionalDesignation? CreatingItem => _item;

    public override Task ExecuteAsync() {
        _item = new ConditionalDesignation("", "");
        _context.Items.Add(_item);
        _context.SelectedItem = _item;
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _context.Items.Remove(_item);
        _context.SelectedItem = null;
        _item = null;
        return Task.CompletedTask;
    }

    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}

public class FocusingCommand : DeferredCommand<CognDesignGridWindow, ConditionalDesignation>, IDataGridRequester
{
    protected readonly CursorPositionService<System.Windows.Controls.DataGrid> _cursorPositionService;
    protected DataGridMemento? _dataGridMemento;
    public FocusingCommand(CognDesignGridWindow context, CursorPositionService<System.Windows.Controls.DataGrid> cursorPositionService, ConditionalDesignation item) : base(context) {
        _cursorPositionService = cursorPositionService;
        _item = item;
    }

    public System.Windows.Controls.DataGrid DataGrid { get; set; }

    public override Task ExecuteAsync() {
        EventBus<IVmGlobalSubscriber>.RaiseEvent<IDataGridRequestResolver>(r => r.GetGrid(this));
        _dataGridMemento = _cursorPositionService.FocusAndEditItem(DataGrid, _item!);
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _dataGridMemento!.Restore(DataGrid); // получается запомнить фокус прошлый
        return Task.CompletedTask;
    }

    public override Task ExecuteDeferredAsync() => Task.CompletedTask;
}