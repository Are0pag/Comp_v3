using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Infrastructure;
using WPF.Extensions.View.Elements;
using WPF.Templates.TableWindow.v1.Entities;

namespace WPF.Templates.TableWindow.v1.Operations.Commands.Ui;

public class FocusCellCommand<TWindow, T> : BaseCommand<T>
    where TWindow : Window
    where T : class, new()
{
    protected readonly ModuleContext<TWindow, T> _moduleContext;

    public FocusCellCommand(T parameter, ModuleContext<TWindow, T> moduleContext) : base(parameter) {
        _moduleContext = moduleContext;
    }

    public override async Task ExecuteAsync() {
        var dg = _moduleContext.DataGrid;

        // Переносим выполнение в UI-поток с правильным приоритетом
        await dg.Dispatcher.InvokeAsync(async () => {
            try {
                var column = dg.GetFirstEditableColumn();
                if (column == null)
                    return;

                dg.ScrollIntoView(_parameter);

                // 2. Даем WPF время обновить визуальное дерево после скролла
                await Task.Delay(50);

                // 3. Получаем строку (убедитесь, что ваш метод GetRowFromItemAsync учитывает виртуализацию)
                var row = await dg.GetRowFromItemAsync(_parameter!);
                if (row == null)
                    return;

                // 4. Получаем ячейку
                var cell = dg.GetCell(row, column);
                if (cell == null)
                    return;

                // 5. Устанавливаем текущую ячейку и фокус
                dg.CurrentCell = new DataGridCellInfo(_parameter!, column);

                cell.Focus();

                // 6. Вызываем редактирование чуть позже, когда фокус окончательно закрепился
                await dg.Dispatcher.InvokeAsync(() => { dg.BeginEdit(); }, DispatcherPriority.Input);
            }
            catch (Exception e) {
                e.Log(this);
                throw;
            }
        }, DispatcherPriority.Background); // Background гарантирует, что данные уже привязались к UI
    }
}