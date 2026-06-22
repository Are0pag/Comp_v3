using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.Contracts;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

public partial class ButtonVmCancel<TWindow, T> : BaseButtonsVm<TWindow, T, ActionCancel<TWindow, T>>
    where TWindow : Window
    where T : class, IDbEntity
{
    public ButtonVmCancel(ActionCancel<TWindow, T> context) : base(context) {
    }

    [RelayCommand(CanExecute = nameof(CanCancel))]
    protected async Task Cancel() {
        await _context.PerformAsync();
    }

    protected bool CanCancel() => _context.CanPerform();
    public override void NotifyCanExecute() {
        CancelCommand.NotifyCanExecuteChanged();
    }
}