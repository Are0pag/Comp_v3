using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Comp_v4;

public partial class TargetWindow : Window
{
    public TargetWindow() {
        InitializeComponent();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        throw new NotImplementedException();
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        throw new NotImplementedException();
    }

    private void InfoDataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e) {
        throw new NotImplementedException();
    }
}