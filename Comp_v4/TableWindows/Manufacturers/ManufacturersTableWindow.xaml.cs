using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.Events.Requests;

namespace Comp_v4.TableWindows.Manufacturers;

public partial class ManufacturersTableWindow : Window, IDisposable, IDataGridRequestResolver<ManufacturersTableWindow>
{
    public ManufacturersTableWindow() {
        InitializeComponent();
    }
    
    public void Dispose() => EventBus<IGlobSubscriber>.Unsubscribe(this);

    public required Guid Id { get; init; }
    
    void IDataGridRequestResolver<ManufacturersTableWindow>.GetGrid(IDataGridRequester<ManufacturersTableWindow> requester) {
        requester.DataGrid = MainDataGrid;
    }


    private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IPreviewKeyDownHandler>(h => h.OnPreviewKeyDown(sender, e));
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IPreviewKeyDownHandler>(h => h.OnPreviewMouseDown(sender, e));
    }

    private void MainDataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<ICellEditHandler>(h => h.OnBeginning(sender, e));
    }

    private void MainDataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<ICellEditHandler>(h => h.OnEnding(sender, e));
    }

    private void FilterTextBox_GotFocus(object sender, RoutedEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringInputHandler>(h => h.OnUserStartFiltering());
    }

    private void FilterTextBox_LostFocus(object sender, RoutedEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringInputHandler>(h => h.OnUserEndFiltering());
    }
}