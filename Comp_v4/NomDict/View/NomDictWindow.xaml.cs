using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Comp_v4.NomDict.View;

public partial class NomDictWindow : Window
{
    public NomDictWindow() {
        InitializeComponent();
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        throw new NotImplementedException();
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        throw new NotImplementedException();
    }

    private void MainDataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        throw new NotImplementedException();
    }

    private void MainDataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e) {
        throw new NotImplementedException();
    }
}