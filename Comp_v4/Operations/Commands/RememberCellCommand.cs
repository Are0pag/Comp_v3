using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates;

namespace Comp_v4.Operations.Commands;

public class RememberCellCommand : BaseCommand
{
    protected readonly Cell _cell;
    protected DataGridCellInfo _cellInfo;
    
    public RememberCellCommand(ModuleContext context) : base(context) {
        _cell = App.Host.Services.GetRequiredService<Cell>();
    }

    public override async Task ExecuteAsync() {
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                //_context.DataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
                _cellInfo = _context.DataGrid.CurrentCell;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }

    public override async Task UndoAsync() {
        await Task.Delay(100);
        await App.Current.Dispatcher.InvokeAsync(() => {
            try {
                // Расширенная диагностика
                Console.WriteLine($"Current Cell Info Details:");
                Console.WriteLine($"Item: {_cellInfo.Item}");
                Console.WriteLine($"Column: {_cellInfo.Column}");
                Console.WriteLine($"Is Valid: {_cellInfo.IsValid}");

                
                // Проверка на null и валидность
                if (_cellInfo.Item == null || _cellInfo.Column == null) {
                    Console.WriteLine("Cell Info is not valid: Item or Column is null");
                    return;
                }
                
                // Принудительно меняем режим выделения
                _context.DataGrid.SelectionUnit = DataGridSelectionUnit.Cell;
                _context.DataGrid.SelectionMode = DataGridSelectionMode.Single;

                // Очищаем текущий выбор
                _context.DataGrid.UnselectAll();

                // Устанавливаем нужную ячейку
                _context.DataGrid.CurrentCell = _cellInfo;
                _context.DataGrid.SelectedCells.Clear();
                _context.DataGrid.SelectedCells.Add(_cellInfo);
        
                // Фокусировка
                _context.DataGrid.Focus();
                _context.DataGrid.BeginEdit();
                
                var args = new MouseButtonEventArgs(Mouse.PrimaryDevice, Environment.TickCount, MouseButton.Left) {
                    RoutedEvent = Mouse.MouseDownEvent
                };
        
                _context.DataGrid.RaiseEvent(args);
                args.RoutedEvent = Mouse.MouseUpEvent;
                _context.DataGrid.RaiseEvent(args);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: {ex.Message}");
            }
        });
    }
}