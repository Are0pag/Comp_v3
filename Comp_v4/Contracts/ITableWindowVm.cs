using System.Windows.Controls;
using System.Windows.Input;

namespace WPF.Templates.Contracts;

public interface ITableWindowVm
{
    void Window_PreviewKeyDown(object sender, KeyEventArgs e);
    void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e);
    void InfoDataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e);
}