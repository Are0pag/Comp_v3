using System.Windows.Controls;
using System.Windows.Input;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.Core;

public interface IViewModel : ICoreContract { }

public interface IState : ICoreContract
{
    bool CanAddItem(DataGridViewModel vm);

    bool CanDeleteItem(DataGridViewModel vm);

    bool CanSaveChanges();

    bool CanEditItem(DataGridCellEditEndingEventArgs e);
}

public interface ICoreContract
{
    Task AddItemAsync(DataGridViewModel context);
    
    Task DeleteItemAsync(DataGridViewModel context);
    
    Task EditItemAsync(DataGridViewModel context);
    
    Task SaveChanges();

    Task OnCellEditEnding(DataGridViewModel vm, object? sender, DataGridCellEditEndingEventArgs e);

    Task OnHandleKeyInput(DataGridViewModel vm, object? sender, KeyEventArgs e);

    Task CancelNewItemAdding();
}