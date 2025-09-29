using System.Windows.Controls;
using System.Windows.Data;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;

namespace Comp_v4.NomDict.Vm;

public class TreeViewVm
{
    protected readonly DataGridVm _dataGridVm;
    protected Category? _selectedCategory;
    
    public TreeViewVm(DataGridVm dataGridVm) {
        _dataGridVm = dataGridVm;
        CollectionViewSource.GetDefaultView(_dataGridVm.Items);
    }

    protected virtual bool ItemsFilter(object item) {
        return _selectedCategory != null && ((Component) item).Category == _selectedCategory;
    }
    
    private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e) {
        //_selectedCategory = e
        CollectionViewSource.GetDefaultView(_dataGridVm.Items).Refresh();
    }
}