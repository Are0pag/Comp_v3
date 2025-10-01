using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.NomDict.View;

public partial class NomDictWindow : Window, IDisposable
{
    public NomDictWindow(TreeViewVm treeViewVm, DataGridVm dataGridVm, 
                         AddNewCategoryButtonVm addNewCategoryButtonVm, DeleteCategoryButtonVm deleteCategoryButtonVm, UpdateCategoryNameButtonVm updateCategoryNameButtonVm) {
        InitializeComponent();
        CategoryTreeView.DataContext = treeViewVm;
        MainDataGrid.DataContext = dataGridVm;
        TreeView_Button_Add.DataContext = addNewCategoryButtonVm;
        TreeView_Button_Delete.DataContext = deleteCategoryButtonVm;
        TreeView_Button_UpdateName.DataContext = updateCategoryNameButtonVm;
    }

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e) {
        
    }

    private void Window_OnPreviewMouseDown(object sender, MouseButtonEventArgs e) {
        EventBus<IGlobalButtonEvent>.RaiseEvent<INotifyConditionalsChanged>(n => n?.NotifyCanExecute());
    }

    private void DataGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        
    }

    public void Dispose() {
        
    }

    public void OnDataGridDoubleClick(object args) {
        EventBus<INomDictWindowSubscriber>.RaiseEvent<IDataGridDoubleClickHandler>(h => h?.OnDataGridDoubleClick(MainDataGrid));
    }

    private void CategoriesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
        EventBus<INomDictWindowSubscriber>.RaiseEvent<ISelectedCategoryChangedHandler>(h => h?.OnSelectedCategoryChanged(CategoryTreeView));
    }
}