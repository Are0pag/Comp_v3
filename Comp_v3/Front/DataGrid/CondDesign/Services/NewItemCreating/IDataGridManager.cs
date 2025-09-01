using System.Collections.ObjectModel;

namespace Comp_v3.Front.DataGrid.CondDesign.Services.NewItemCreating;

public interface IDataGridManager<T> where T : class
{
    public ObservableCollection<T> Items { get; }
    
}