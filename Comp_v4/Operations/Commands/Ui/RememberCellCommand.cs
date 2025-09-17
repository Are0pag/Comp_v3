using System.Windows.Controls;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberCellCommand : BaseCommand<DataGridBeginningEditEventArgs>
{
    protected readonly ModuleContext _moduleContext;
    protected DataGridCell? _cell;
    
    public RememberCellCommand(DataGridBeginningEditEventArgs parameter, ModuleContext moduleContext) : base(parameter) {
        _moduleContext = moduleContext;
    }

    public override async Task ExecuteAsync() {
        _cell = _moduleContext.DataGrid.GetCell(_parameter.Row, _parameter.Column);
    }

    public override async Task UndoAsync() {
        await Task.Delay(100);
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                _cell!.Focus();
                _moduleContext.DataGrid.SelectedItem = _cell;
                _moduleContext.DataGrid.BeginEdit();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }
}