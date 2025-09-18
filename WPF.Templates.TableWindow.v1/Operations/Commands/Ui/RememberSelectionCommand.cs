using System.Windows;
using Comp.ModelData.TechnicalItems;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberSelectionCommand<TWindow, T> : BaseCommand<object>
    where TWindow : Window
    where T : class
{
    protected readonly ModuleContext<TWindow, T> _moduleContext;
    protected T? _item;
    
    public RememberSelectionCommand(object parameter, ModuleContext<TWindow, T> moduleContext) : base(parameter) {
        _moduleContext = moduleContext;
    }

    public override Task ExecuteAsync() {
        _item = _moduleContext.DataGridViewModel.SelectedItem;
        return Task.CompletedTask;
    }

    public override Task UndoAsync() {
        _moduleContext.DataGridViewModel.SelectedItem = _item;
        _moduleContext.DataGrid.Focus();
        return Task.CompletedTask;
    }
}