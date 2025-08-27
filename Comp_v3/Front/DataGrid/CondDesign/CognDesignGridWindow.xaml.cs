using System.Windows;
using System.Windows.Controls;

namespace Comp_v3.Front.DataGrid.CondDesign;

public partial class CognDesignGridWindow : Window
{
    public CognDesignGridWindow(CognDesignGridVm cognDesignGridVm) {
        InitializeComponent();
        DataContext = cognDesignGridVm;
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        
    }
}