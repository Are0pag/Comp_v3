namespace WPF.Templates;

public class GridContentEditor
{
    public GridContentEditor(ActionAddItem actionAddItem, ActionEditItem actionEditItem, ActionDeleteItem actionDeleteItem) {
        AddItem = actionAddItem;
        EditItem = actionEditItem;
        DeleteItem = actionDeleteItem;
    }

    public ActionAddItem AddItem { get; }

    public ActionEditItem EditItem { get; }

    public ActionDeleteItem DeleteItem { get; }
}

public class GridEventsHandler
{
    
}

public class TableController
{
    protected readonly GridContentEditor _gridEditor;

    public TableController(GridContentEditor gridEditor) {
        _gridEditor = gridEditor;
    }
}