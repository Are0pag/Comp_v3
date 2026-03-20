using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

public partial class ButtonVmSave<TWindow, T> : BaseButtonsVm<TWindow, T, ActionSave<TWindow, T>>
    where TWindow : Window
    where T : class, IDbEntity
{
    public ButtonVmSave(ActionSave<TWindow, T> context) : base(context) {
        
    }

    [RelayCommand(CanExecute = nameof(CanSave))]
    protected async Task Save() => await _context.PerformAsync();
    protected bool CanSave() => _context.CanPerform();
    public override void NotifyCanExecute() {
        SaveCommand.NotifyCanExecuteChanged();
    }
}