using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.NomDict.Events;
using Utils.EventBus;

namespace Comp_v4.NomDict.View;

public partial class NomDictWindow : Window, IDisposable, IDataGridDoubleClickHandler
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

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        
    }

    public void Dispose() {
        
    }

    public void OnDataGridDoubleClick(object args) {
        EventBus<INomDictWindowSubscriber>.RaiseEvent<IDataGridDoubleClickHandler>(h => h?.OnDataGridDoubleClick(MainDataGrid));
    }
}