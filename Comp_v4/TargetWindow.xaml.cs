using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Utils.EventBus;
using WPF.Templates;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.Events.Requests;
using WPF.Templates.TableWindow.Vm;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4;

public partial class TargetWindow : Window, IDisposable, IDataGridRequestResolver<TargetWindow>
{
    protected readonly DataGridViewModel _dataGridViewModel;
    
    public TargetWindow(DataGridViewModel dataGridViewModel, FiltersVm filtersVm, ButtonVmAddItem buttonVmAddItem, ButtonVmSave buttonVmSave, ButtonVmDeleteItem buttonVmDeleteItem) {
        InitializeComponent();
        _dataGridViewModel = dataGridViewModel;
        MainDataGrid.DataContext = _dataGridViewModel;
        FiltersStackPanel.DataContext = filtersVm;
        
        IgnoreCaseCheckBox.DataContext = filtersVm;

        AddNewItemButton.DataContext = buttonVmAddItem;
        SaveChangesButton.DataContext = buttonVmSave;
        DeleteItemButton.DataContext = buttonVmDeleteItem;

        InfoDataGridContextMenuAddNewItemCommand.DataContext = buttonVmAddItem;
        InfoDataGridContextMenuDeleteItemCommand.DataContext = buttonVmDeleteItem;
        EventBus<IGlobSubscriber>.Subscribe(this);
    }

    public void Dispose() => EventBus<IGlobSubscriber>.Unsubscribe(this);

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IPreviewKeyDownHandler>(h => h.OnPreviewKeyDown(sender, e));
    }

    private void InfoDataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e) {
       EventBus<IGlobSubscriber>.RaiseEvent<ICellEditHandler>(h => h.OnBeginning(sender, e));
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<ICellEditHandler>(h => h.OnEnding(sender, e));
    }

    void IDataGridRequestResolver<TargetWindow>.GetGrid(IDataGridRequester<TargetWindow> requester) => requester.DataGrid = MainDataGrid;

    private void TargetWindow_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IPreviewKeyDownHandler>(h => h.OnPreviewMouseDown(sender, e));
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringInputHandler>(h => h.OnUserStartFiltering());
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IFilteringInputHandler>(h => h.OnUserEndFiltering());
    }
}