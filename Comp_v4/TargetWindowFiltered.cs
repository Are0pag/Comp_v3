using WPF.Templates;
using WPF.Templates.TableWindow.Vm;

namespace Comp_v4;

public class TargetWindowFiltered : TargetWindow
{
    public TargetWindowFiltered(DataGridViewModelFiltered dataGridViewModel, ButtonVmAddItem buttonVmAddItem, ButtonVmSave buttonVmSave, ButtonVmDeleteItem buttonVmDeleteItem) 
        : base(dataGridViewModel, buttonVmAddItem, buttonVmSave, buttonVmDeleteItem) 
    {
        CaseSensitiveCheckBox.DataContext = dataGridViewModel;
        ClearFiltersButton.DataContext = dataGridViewModel;
        DataContext = dataGridViewModel;
    }
}