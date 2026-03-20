using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

public partial class ButtonVmDeleteItem<TWindow, T> : BaseButtonsVm<TWindow, T, ActionDeleteItem<TWindow, T>>
    where TWindow : Window
    where T : class, IDbEntity
{
    public ButtonVmDeleteItem(ActionDeleteItem<TWindow, T> context) : base(context) {
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    protected async Task Delete() => await _context.PerformAsync();
    protected bool CanDelete() => _context.CanPerform();
    public override void NotifyCanExecute() {
        DeleteCommand.NotifyCanExecuteChanged();
    }
}