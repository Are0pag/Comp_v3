using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WPF.Services.View.AutoNavigation.Focusing;

public abstract class CursorPositionService<T> 
    where T : FrameworkElement
{
    protected readonly Mover _mover = new Mover();
    public abstract DataGridMemento FocusAndEditFirstEditableItem(T container, object item);
    public abstract DataGridMemento FocusItem(T container, object item);
    public abstract DataGridMemento RememberCursorPos(T container);

    public abstract CellMemento FocusAndEditItem(DataGrid dataGrid, DataGridCellEditEndingEventArgs e);
    
    public virtual DataGrid MoveCursorToLeftCell(DataGrid dataGrid) => _mover.MoveToLeftCell(dataGrid);
}