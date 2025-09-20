using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
using WPF.Templates;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.Events.Requests;
using WPF.Templates.TableWindow.Vm;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4;

public partial class TargetWindow : Window, IDisposable, IDataGridRequestResolver<TargetWindow>
{
    protected readonly DataGridViewModel<ConditionalDesignation> _dataGridViewModel;
    protected readonly FiltersVmCd _filtersVm;
    protected readonly DummyWindowEventsHandler<TargetWindow, ConditionalDesignation, FiltersVmCd> _eventBus;
    
    public TargetWindow(DataGridViewModel<ConditionalDesignation> dataGridViewModel, FiltersVmCd filtersVm, 
                        ButtonVmAddItem<TargetWindow, ConditionalDesignation> buttonVmAddItem, 
                        ButtonVmSave<TargetWindow, ConditionalDesignation> buttonVmSave, 
                        ButtonVmDeleteItem<TargetWindow, ConditionalDesignation> buttonVmDeleteItem,
                        DummyWindowEventsHandler<TargetWindow, ConditionalDesignation, FiltersVmCd> eventManager) 
    {
        InitializeComponent();

        Id = new Guid();
        _dataGridViewModel = dataGridViewModel;
        MainDataGrid.DataContext = _dataGridViewModel;
        FiltersStackPanel.DataContext = filtersVm;
        _filtersVm = filtersVm;
        
        IgnoreCaseCheckBox.DataContext = filtersVm;

        AddNewItemButton.DataContext = buttonVmAddItem;
        SaveChangesButton.DataContext = buttonVmSave;
        DeleteItemButton.DataContext = buttonVmDeleteItem;

        InfoDataGridContextMenuAddNewItemCommand.DataContext = buttonVmAddItem;
        InfoDataGridContextMenuDeleteItemCommand.DataContext = buttonVmDeleteItem;
        _eventBus = eventManager;
        EventBus<IGlobSubscriber>.Subscribe(this);
    }

    public void Dispose() => EventBus<IGlobSubscriber>.Unsubscribe(this);

    public required Guid Id { get; init; }
    
    void IDataGridRequestResolver<TargetWindow>.GetGrid(IDataGridRequester<TargetWindow> requester) => requester.DataGrid = MainDataGrid;


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