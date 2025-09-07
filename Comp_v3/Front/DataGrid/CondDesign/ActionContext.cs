namespace Comp_v3.Front.DataGrid.CondDesign;

public enum ActionContextType
{
    Default,
    GoToEditingStateAfterUndoNewItemCreation,
}

public static class ActionContext
{
    public static ActionContextType ActionContextType { get; set; }
}