using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.TableWindows.Manufacturers;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Events;
using WPF.Templates.TableWindow.v1.Events.Requests;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;
using WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

namespace Comp_v4.TableWindows.TypeSizes;

public partial class TypeSizesTableWindow : Window, IDisposable, IDataGridRequestResolver<TypeSizesTableWindow>
{
    public TypeSizesTableWindow(DataGridViewModel<Comp.ModelData.TechnicalItems.TypeSize> dataGridViewModel, 
                                FiltersVmBase filtersVm, 
                        
                                ButtonVmAddItem<TypeSizesTableWindow, Comp.ModelData.TechnicalItems.TypeSize> buttonVmAddItem, 
                                ButtonVmSave<TypeSizesTableWindow, Comp.ModelData.TechnicalItems.TypeSize> buttonVmSave, 
                                ButtonVmDeleteItem<TypeSizesTableWindow, Comp.ModelData.TechnicalItems.TypeSize> buttonVmDeleteItem) {
        InitializeComponent();
        MainDataGrid.DataContext = dataGridViewModel;
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

    public required Guid Id { get; init; }
    
    void IDataGridRequestResolver<TypeSizesTableWindow>.GetGrid(IDataGridRequester<TypeSizesTableWindow> requester) {
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