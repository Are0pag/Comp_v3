using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

public partial class ButtonVmAddItem<TWindow, T> : BaseButtonsVm<TWindow, T, ActionStartAddingNewItem<TWindow, T>>
    where TWindow : Window
    where T : class, IDbEntity, new()
{
    public ButtonVmAddItem(ActionStartAddingNewItem<TWindow, T> context) : base(context) {
    }

    [RelayCommand(CanExecute = nameof(CanAddItem))]
    protected async Task AddItem() => await _context.PerformAsync();
    protected bool CanAddItem() => _context.CanPerform();
    public override void NotifyCanExecute() => AddItemCommand.NotifyCanExecuteChanged();
}