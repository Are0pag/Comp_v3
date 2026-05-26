using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Comp_v4.CompCard.Events;
using Comp_v4.TableWindows.TypeSizes.Events;
using Comp_v4.TableWindows.TypeSizes.Vm.Buttons;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Events;
using WPF.Templates.TableWindow.v1.Events.Requests;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;
using WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

namespace Comp_v4.TableWindows.TypeSizes;

using W = TypeSizesTableWindow;
using T = TypeSize;

public partial class TypeSizesTableWindow : Window, IDisposable, IDataGridRequestResolver<W>, ITableWindowHandler
{
    protected readonly EditTsButVm _editTsButVm;
    public TypeSizesTableWindow(DataGridViewModel<T> dataGridViewModel, 
                                IFilter<T, FiltersVmBase> filtersVm, 
                        
                                ButtonVmAddItem<W, T> buttonVmAddItem, 
                                ButtonVmSave<W, T> buttonVmSave, 
                                ButtonVmDeleteItem<W, T> buttonVmDeleteItem, 
                                EditTsButVm editTsButVm) {
        InitializeComponent();
        _editTsButVm = editTsButVm;
        EditButton.DataContext = editTsButVm;
        MainDataGrid.DataContext = dataGridViewModel;
        FiltersStackPanel.DataContext = filtersVm;
        IgnoreCaseCheckBox.DataContext = filtersVm;

        AddNewItemButton.DataContext = buttonVmAddItem;
        //SaveChangesButton.DataContext = buttonVmSave;
        DeleteItemButton.DataContext = buttonVmDeleteItem;

        InfoDataGridContextMenuAddNewItemCommand.DataContext = buttonVmAddItem;
        InfoDataGridContextMenuDeleteItemCommand.DataContext = buttonVmDeleteItem;
        EventBus<IGlobSubscriber>.Subscribe(this);
        EventBus<ICompCardSubscriber>.Subscribe(this);
    }
    
    
    public void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    public void HandleClosingTableWindow<T>(object? args) where T : Window {
        Application.Current.Dispatcher.BeginInvoke(Close);
    }

    public required Guid Id { get; init; }
    
    void IDataGridRequestResolver<W>.GetGrid(IDataGridRequester<W> requester) {
        requester.DataGrid = MainDataGrid;
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IPreviewKeyDownHandler>(h => h.OnPreviewKeyDown(sender, e));
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobSubscriber>.RaiseEvent<IPreviewKeyDownHandler>(h => h.OnPreviewMouseDown(sender, e));
        _editTsButVm.NotifyCanExecute();
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

    private void MainDataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        var dataGrid = (DataGrid)sender;
        var hit = VisualTreeHelper.HitTest(dataGrid, e.GetPosition(dataGrid));
        var cell = hit?.VisualHit.GetParentOfType<DataGridCell>();

        if (cell?.Column is DataGridTemplateColumn or DataGridCheckBoxColumn) {
            _ = HandleDoubleClick(sender, e);
        }
    }

    private async Task HandleDoubleClick(object sender, MouseButtonEventArgs e) {
        // Ждем один цикл диспетчеризации
        await Application.Current.Dispatcher.InvokeAsync( async () => {
            if (MainDataGrid.SelectedItem != null) {
                await _editTsButVm.OnClickAsync();
            }
                
        }, DispatcherPriority.Background);
    }
}

// Расширительный метод для упрощения поиска родительского элемента
public static class VisualTreeHelperExtensions
{
    public static T GetParentOfType<T>(this DependencyObject child) where T : DependencyObject {
        var parentObject = VisualTreeHelper.GetParent(child);

        while (parentObject != null) {
            if (parentObject is T parent)
                return parent;

            parentObject = VisualTreeHelper.GetParent(parentObject);
        }

        return null;
    }
}