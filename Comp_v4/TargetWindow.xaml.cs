using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm;

namespace Comp_v4;

public partial class TargetWindow : Window
{
    protected readonly DataGridViewModel _dataGridViewModel;
    public TargetWindow(DataGridViewModel dataGridViewModel, ButtonVmAddItem buttonVmAddItem) {
        InitializeComponent();
        _dataGridViewModel = dataGridViewModel;
        MainDataGrid.DataContext = _dataGridViewModel;

        AddNewItemButton.DataContext = buttonVmAddItem;
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