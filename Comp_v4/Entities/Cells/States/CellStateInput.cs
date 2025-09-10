using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.Entities;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;
using WPF.Extensions.View.Elements;
using WPF.Services.UserActionsHandling.InputText;

namespace WPF.Templates.TableWindow.States;

public class CellStateInput : BaseCellState
{
    protected readonly DataGridPropertyRestoreService<ConditionalDesignation> _propertyRestoreService;
    protected DataGridCellEditEndingEventArgs? _cellEditEndingEventArgs;
    
    public CellStateInput(IModuleCommandScheduler scheduler, ModuleContext context, 
                          DataGridPropertyRestoreService<ConditionalDesignation> propertyRestoreService) : base(scheduler, context) {
        _propertyRestoreService = propertyRestoreService;
    }

    public override void OnBeginning(object? sender, DataGridBeginningEditEventArgs e) {
        base.OnBeginning(sender, e);
    }

    public override void OnEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        _cellEditEndingEventArgs = e;
    }

    public override void OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Tab:
            case Key.Enter:
                if (_context.DataGrid.SelectedItem is not ConditionalDesignation conditionalDesignation)
                    throw new InvalidCastException();
                
                _propertyRestoreService.RememberValue(
                        conditionalDesignation,
                        _context.DataGrid.CurrentCell.Column.GetPropertyName()
                        );
            break;
        }
    }
}