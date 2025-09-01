using Utils.WPF.Mvvm;

namespace Comp_v3.Front.DataGrid.CondDesign.Services.NewItemCreating;

public class NewItemCreatingInDataGridService<T> //: NotifyPropertyChanged 
    where T : class, new() /* Тип T должен иметь публичный конструктор без параметров - ДА */
{
    
    public T NewItem { get; set; } = new();
}