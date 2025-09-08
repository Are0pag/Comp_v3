using System.Windows.Controls;
using System.Windows.Input;
using WPF.Templates.TableWindow.Vm;

namespace WPF.Templates.Core;

public interface IViewModel : ICoreContract { }

public interface IState : ICoreContract
{
    bool CanAddItem(ViewModel vm);

    bool CanDeleteItem(ViewModel vm);

    bool CanSaveChanges();

    bool CanEditItem(DataGridCellEditEndingEventArgs e);
}

public interface ICoreContract
{
    Task AddItemAsync(ViewModel context);
    
    Task DeleteItemAsync(ViewModel context);
    
    Task EditItemAsync(ViewModel context);
    
    Task SaveChanges();

    Task OnCellEditEnding(ViewModel vm, object? sender, DataGridCellEditEndingEventArgs e);

    Task OnHandleKeyInput(ViewModel vm, object? sender, KeyEventArgs e);

    Task CancelNewItemAdding();
}