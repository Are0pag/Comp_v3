using System.Windows.Controls;
using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp_v3.Front.DataGrid.CondDesign.Window;
using Comp_v3.Front.Events;
using Comp_v3.Front.Events.VmInvoking.Request;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using WPF.Extensions.View.DataGrid;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.View.AutoNavigation.Focusing;

namespace Comp_v3.Front.DataGrid.CondDesign.Commands;

public class UpdateItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    
    protected readonly IPropertyValueRestoreService<ConditionalDesignation> _propertyValueRestoreService;
    protected readonly DataGridCellEditEndingEventArgs _e;
    protected readonly System.Windows.Controls.DataGrid _grid;
    protected CellMemento? _gridMementoOnEditing;

    public UpdateItemCommand(CognDesignGridVm context, ConditionalDesignation item, DataGridCellEditEndingEventArgs e, System.Windows.Controls.DataGrid dg) : base(context) {
        _e = e; _item = item; _grid = dg;
        //_cursorPositionService = App.Host.Services.GetRequiredService<CursorPositionService<System.Windows.Controls.DataGrid>>();
        _propertyValueRestoreService = App.Host.Services.GetRequiredService<IPropertyValueRestoreService<ConditionalDesignation>>();
    }

    public override Task ExecuteAsync() {
        _propertyValueRestoreService.RememberValue(_item!, _e.Column.GetPropertyName());
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        //_propertyValueRestoreService.RollbackEdit();
        
        return Task.CompletedTask;
    }

    public override async Task ExecuteDeferredAsync() {
        await _context.Repository.UpdateAsync(_item);
    }
}

public class FocusOnUpdatingItemCommand : DeferredCommand<CognDesignGridVm, ConditionalDesignation>
{
    protected readonly CursorPositionService<System.Windows.Controls.DataGrid> _cursorPositionService;
    public FocusOnUpdatingItemCommand(CognDesignGridVm context) : base(context) {
    }

    public override async Task ExecuteAsync() {
        
    }

    public override async Task UndoAsync() {
        //_cursorPositionService.FocusAndEditItem(_grid, _e);
    }

    public override async Task ExecuteDeferredAsync() {
        throw new NotImplementedException();
    }
}
