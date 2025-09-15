using WPF.Templates;
using WPF.Templates.TableWindow.Vm;

namespace Comp_v4;

public class TargetWindowFiltered : TargetWindow
{
    public TargetWindowFiltered(DataGridViewModelFiltered dataGridViewModel, 
                                ButtonVmAddItem buttonVmAddItem, 
                                ButtonVmSave buttonVmSave, 
                                ButtonVmDeleteItem buttonVmDeleteItem) 
        : base(dataGridViewModel, buttonVmAddItem, buttonVmSave, buttonVmDeleteItem) 
    {
        // Устанавливаем DataContext для всего окна
        DataContext = dataGridViewModel;
        
        // Привязываем элементы управления фильтрами
        CaseSensitiveCheckBox.DataContext = dataGridViewModel;
        ClearFiltersButton.DataContext = dataGridViewModel;
        
        // Убедитесь, что DataGrid использует FilteredItems
        MainDataGrid.ItemsSource = dataGridViewModel.FilteredItems;
    }
}