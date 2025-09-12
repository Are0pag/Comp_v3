using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Extensions.View.Elements;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberCellCommand : BaseCommand<DataGridCellEditEndingEventArgs>
{
    protected DataGridCell? _cell;
    public RememberCellCommand(DataGridCellEditEndingEventArgs parameter) : base(parameter) {
    }

    public override async Task ExecuteAsync() {
        if (_parameter!.EditingElement is not DataGridCell cell) {
            new InvalidOperationException().Log(this);
            return;
        }
        _cell = cell;
    }

    public override async Task UndoAsync() {
        await Task.Delay(100);
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                _cell!.Focus();
                _moduleContext.DataGrid.BeginEdit();
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }
}