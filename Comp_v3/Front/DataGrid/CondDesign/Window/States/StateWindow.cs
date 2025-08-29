using System.Windows.Controls;

namespace Comp_v3.Front.DataGrid.CondDesign.Window.States;

public abstract class StateWindow 
{
    public virtual void Entry(CognDesignGridWindow window) { }

    public virtual void OnCellEditEnding(CognDesignGridWindow window, object? sender, DataGridCellEditEndingEventArgs e) { }

    /// <summary>
    /// INewValueTryAddingToDataGridHandler involication
    /// </summary>
    /// <param name="window"></param>
    /// <param name="newValue"></param>
    public virtual void OneNewValueAdded(CognDesignGridWindow window, object newValue) { }

    public virtual void Exit(CognDesignGridWindow window) { }

}