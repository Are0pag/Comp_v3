using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.CompCard.Events;
using Utils.EventBus;
using WPF.Templates.TableWindow.v1.Events;
using WPF.Templates.TableWindow.v1.Events.Requests;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;
using WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

namespace Comp_v4.TableWindows.ConditionalDesignation;

public partial class CondDesignTableWindow : Window, IDisposable, IDataGridRequestResolver<CondDesignTableWindow>, ITableWindowHandler
{
    protected readonly DataGridViewModel<Comp.ModelData.TechnicalItems.ConditionalDesignation> _dataGridViewModel;
    protected readonly FiltersVmBase _filtersVm;
    
    public CondDesignTableWindow(DataGridViewModel<Comp.ModelData.TechnicalItems.ConditionalDesignation> dataGridViewModel, 
                        FiltersVmBase filtersVm, 
                        
                        ButtonVmAddItem<CondDesignTableWindow, Comp.ModelData.TechnicalItems.ConditionalDesignation> buttonVmAddItem, 
                        ButtonVmSave<CondDesignTableWindow, Comp.ModelData.TechnicalItems.ConditionalDesignation> buttonVmSave, 
                        ButtonVmDeleteItem<CondDesignTableWindow, Comp.ModelData.TechnicalItems.ConditionalDesignation> buttonVmDeleteItem) 
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
        EventBus<IGlobSubscriber>.Subscribe(this);
        EventBus<ICompCardSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IGlobSubscriber>.Unsubscribe(this);
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }

    public void HandleClosingTableWindow<T>(object? args) where T : Window {
        Application.Current.Dispatcher.BeginInvoke(Close);
    }

    public required Guid Id { get; init; }
    
    void IDataGridRequestResolver<CondDesignTableWindow>.GetGrid(IDataGridRequester<CondDesignTableWindow> requester) => requester.DataGrid = MainDataGrid;


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