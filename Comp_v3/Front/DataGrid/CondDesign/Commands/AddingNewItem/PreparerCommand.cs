using Comp_v3.Front.DataGrid.CondDesign.Grid;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command.Heterochromic;

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