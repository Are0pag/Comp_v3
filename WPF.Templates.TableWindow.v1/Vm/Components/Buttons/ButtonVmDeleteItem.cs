using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.Comp;
using Comp.ModelData.Contracts;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

public partial class ButtonVmDeleteItem<TWindow, T> : BaseButtonsVm<TWindow, T, ActionDeleteItem<TWindow, T>>
    where TWindow : Window
    where T : class, IDbEntity
{
    public ButtonVmDeleteItem(ActionDeleteItem<TWindow, T> context) : base(context) {
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    protected async Task Delete() {
        if (!await _context.TryExecuteAsync<Component>()) {
            MessageBox.Show("Невозможно удалить элемент, так как он ещё используется другим компонентом");
            return;
        }
        await _context.PerformAsync();
    }

    protected bool CanDelete() => _context.CanPerform();
    public override void NotifyCanExecute() {
        DeleteCommand.NotifyCanExecuteChanged();
    }
}