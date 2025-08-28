using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign;

public partial class CognDesignGridWindow : Window, INewValueAddedToDataGridHandler
{
    public CognDesignGridWindow(CognDesignGridVm cognDesignGridVm, DataGridManageButtonsVm dataGridManageButtonsVm) {
        InitializeComponent();
        InfoDataGrid.DataContext = cognDesignGridVm;
        AddNewItemButton.DataContext = dataGridManageButtonsVm;
        DeleteItemButton.DataContext = dataGridManageButtonsVm;
        EventBus<IVmGlobalSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IVmGlobalSubscriber>.Unsubscribe(this);
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellEditEndingHandler>(h => h.HandleCellEdit(sender, e));
    }

    public void HandleNewValueAdded(object newValue) {
        if (newValue is not ConditionalDesignation conditionalDesignation) 
            throw new ArgumentException("New value is not a conditional designation in CognDesignGridWindow");
        InfoDataGrid.Focus();
        InfoDataGrid.ScrollIntoView(conditionalDesignation);
        InfoDataGrid.SelectedItem = conditionalDesignation;

        Dispatcher.BeginInvoke(new Action(() => {
            if (InfoDataGrid.ItemContainerGenerator.ContainerFromItem(conditionalDesignation) is not DataGridRow raw) 
                throw new ArgumentException("CognDesignGridWindow could not find raw Row");
            InfoDataGrid.CurrentCell = new DataGridCellInfo(conditionalDesignation, InfoDataGrid.Columns[0]);
            InfoDataGrid.BeginEdit();
        }), DispatcherPriority.ContextIdle);
    }
}