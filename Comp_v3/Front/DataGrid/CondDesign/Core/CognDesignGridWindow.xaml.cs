using System.Windows;
using System.Windows.Controls;
using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Comp_v3.Front.Events;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign;

public partial class CognDesignGridWindow : Window
{
    public CognDesignGridWindow(CognDesignGridVm cognDesignGridVm, DataGridManageButtonsVm dataGridManageButtonsVm) {
        InitializeComponent();
        InfoDataGrid.DataContext = cognDesignGridVm;
        AddNewItemButton.DataContext = dataGridManageButtonsVm;
        DeleteItemButton.DataContext = dataGridManageButtonsVm;
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellEditEndingHandler>(h => h.HandleCellEdit(sender, e));
    }
}