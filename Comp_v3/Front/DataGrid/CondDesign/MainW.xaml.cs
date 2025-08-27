using System.Windows;
using System.Windows.Controls;

namespace Comp_v3.Front.DataGrid.CondDesign;

public partial class MainW : Window
{
    public MainW(MainVm mainVm) {
        InitializeComponent();
        DataContext = mainVm;
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        
    }
}