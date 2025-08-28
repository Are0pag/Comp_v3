using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Comp_v3.Front.DataGrid.CondDesign.Entities;
using Comp_v3.Front.Events;
using Comp.ModelData.TechnicalItems;
using Component_v2.Tools.EventBus;

namespace Comp_v3.Front.DataGrid.CondDesign;

public partial class CognDesignGridWindow : Window, INewValueAddedToDataGridHandler
{
    public const byte FIRST_EDITABLE_COLUMN_INDEX = 1;
    
    public CognDesignGridWindow(CognDesignGridVm cognDesignGridVm, DataGridManageButtonsVm dataGridManageButtonsVm) {
        InitializeComponent();
        InfoDataGrid.DataContext = cognDesignGridVm;
        AddNewItemButton.DataContext = dataGridManageButtonsVm;
        DeleteItemButton.DataContext = dataGridManageButtonsVm;
        EventBus<IVmGlobalSubscriber>.Subscribe(this);
    }

    public void Dispose() {
        EventBus<IVmGlobalSubscriber>.Unsubscribe(this);
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e) {
        EventBus<IUiGlobalSubscriber>.RaiseEvent<ICellEditEndingHandler>(h => h.HandleCellEdit(sender, e));
    }

    /* Суть в том, что надо при создании нового ряда в таблице (нового экземпляра модели соотв-но)
            автоматически прокрутить до новых ячеек и поставить курсор для редактирования на первую колонку (первую ячейку) */
    public void HandleNewValueAdded(object newValue) {
        if (newValue is not ConditionalDesignation conditionalDesignation) 
            throw new ArgumentException("New value is not a conditional designation in CognDesignGridWindow");
        InfoDataGrid.Focus();
        InfoDataGrid.ScrollIntoView(conditionalDesignation);
        InfoDataGrid.SelectedItem = conditionalDesignation;

        Dispatcher.BeginInvoke(() => {
                                   ManageCursorPositionByColumnsTypes(conditionalDesignation);
                               }, DispatcherPriority.ContextIdle);
    }

    private void ManageCursorPositionByColumnsTypes(ConditionalDesignation conditionalDesignation) {
        if (InfoDataGrid.ItemContainerGenerator.ContainerFromItem(conditionalDesignation) is not DataGridRow row) 
            throw new ArgumentException("CognDesignGridWindow could not find raw Row");
        
        // Находим первую редактируемую колонку
        var editableColumn = InfoDataGrid.Columns
                                         .FirstOrDefault(column => !column.IsReadOnly && column.Visibility == Visibility.Visible);
        if (editableColumn != null) {
            InfoDataGrid.CurrentCell = new DataGridCellInfo(conditionalDesignation, editableColumn);
            InfoDataGrid.BeginEdit();
            return;
        }
        row.Focus();
    }
}
/*
 * Для обработки выхода из режима редактирования в DataGrid есть несколько событий, которые можно использовать:

## 1. Обработка завершения редактирования через события DataGrid

```csharp
// В конструкторе окна или в методе инициализации подпишитесь на события:
InfoDataGrid.PreviewKeyDown += InfoDataGrid_PreviewKeyDown;
InfoDataGrid.CellEditEnding += InfoDataGrid_CellEditEnding;
InfoDataGrid.LostFocus += InfoDataGrid_LostFocus;
```

## 2. Обработчики событий

```csharp
private void InfoDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
{
    if (e.Key == Key.Tab)
    {
        // Проверяем, находимся ли мы в последней редактируемой колонке
        var currentCell = InfoDataGrid.CurrentCell;
        if (currentCell.Column != null)
        {
            var editableColumns = InfoDataGrid.Columns
                .Where(c => !c.IsReadOnly && c.Visibility == Visibility.Visible)
                .ToList();
            
            int currentIndex = editableColumns.IndexOf(currentCell.Column);
            
            // Если это последняя колонка и нажат Tab без Shift (движение вперед)
            if (currentIndex == editableColumns.Count - 1 && !e.KeyboardDevice.IsKeyDown(Key.LeftShift))
            {
                // Завершаем редактирование и выходим из состояния
                InfoDataGrid.CommitEdit();
                EventBus<IVmGlobalSubscriber>.RaiseEvent<IEditingFinishedHandler>(
                    h => h.HandleEditingFinished(CreatingConditionalDesignation));
            }
        }
    }
    else if (e.Key == Key.Enter)
    {
        // Enter также завершает редактирование
        InfoDataGrid.CommitEdit();
        EventBus<IVmGlobalSubscriber>.RaiseEvent<IEditingFinishedHandler>(
            h => h.HandleEditingFinished(CreatingConditionalDesignation));
    }
    else if (e.Key == Key.Escape)
    {
        // Escape отменяет редактирование
        InfoDataGrid.CancelEdit();
        EventBus<IVmGlobalSubscriber>.RaiseEvent<IEditingCancelledHandler>(
            h => h.HandleEditingCancelled(CreatingConditionalDesignation));
    }
}

private void InfoDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
{
    // Если редактирование завершено и это наша создаваемая строка
    if (e.Row.Item == CreatingConditionalDesignation)
    {
        // Можно добавить дополнительную логику проверки
        EventBus<IVmGlobalSubscriber>.RaiseEvent<IEditingFinishedHandler>(
            h => h.HandleEditingFinished(CreatingConditionalDesignation));
    }
}

private void InfoDataGrid_LostFocus(object sender, RoutedEventArgs e)
{
    // Если фокус ушел с DataGrid и мы в режиме редактирования
    if (CreatingConditionalDesignation != null)
    {
        InfoDataGrid.CommitEdit();
        EventBus<IVmGlobalSubscriber>.RaiseEvent<IEditingFinishedHandler>(
            h => h.HandleEditingFinished(CreatingConditionalDesignation));
    }
}
```

## 3. Интерфейсы для событий

```csharp
public interface IEditingFinishedHandler
{
    void HandleEditingFinished(object editedItem);
}

public interface IEditingCancelledHandler
{
    void HandleEditingCancelled(object cancelledItem);
}
```

## 4. Дополнительная проверка в состоянии

В вашем StateMachine можно добавить проверку:

```csharp
public class StateDgCreatingNewItem : StateDataGrid
{
    public ConditionalDesignation CreatingConditionalDesignation { get; protected set; }

    public override void Entry(CognDesignGridVm vm) {
        CreatingConditionalDesignation = new ConditionalDesignation();
        vm.Items.Add(CreatingConditionalDesignation);
        vm.SelectedItem = CreatingConditionalDesignation;
        
        EventBus<IVmGlobalSubscriber>.RaiseEvent<INewValueAddedToDataGridHandler>(
            h => h.HandleNewValueAdded(CreatingConditionalDesignation));
    }

    public override void Exit(CognDesignGridVm vm) {
        // Очищаем ссылку при выходе из состояния
        CreatingConditionalDesignation = null;
    }
}
```

## 5. Альтернативный подход - через поведение DataGrid

Можно также использовать Attached Property для автоматического определения завершения редактирования:

```csharp
public static class DataGridBehavior
{
    public static readonly DependencyProperty AutoCommitEditProperty = 
        DependencyProperty.RegisterAttached("AutoCommitEdit", typeof(bool), typeof(DataGridBehavior), 
            new PropertyMetadata(false, OnAutoCommitEditChanged));

    public static bool GetAutoCommitEdit(DataGrid obj) => (bool)obj.GetValue(AutoCommitEditProperty);
    public static void SetAutoCommitEdit(DataGrid obj, bool value) => obj.SetValue(AutoCommitEditProperty, value);

    private static void OnAutoCommitEditChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DataGrid dataGrid)
        {
            if ((bool)e.NewValue)
            {
                dataGrid.PreviewLostKeyboardFocus += DataGrid_PreviewLostKeyboardFocus;
                dataGrid.PreviewKeyDown += DataGrid_PreviewKeyDown;
            }
            else
            {
                dataGrid.PreviewLostKeyboardFocus -= DataGrid_PreviewLostKeyboardFocus;
                dataGrid.PreviewKeyDown -= DataGrid_PreviewKeyDown;
            }
        }
    }

    private static void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Логика обработки клавиш
    }

    private static void DataGrid_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        // Логика потери фокуса
    }
}
```

**Основные события для отслеживания:**
- `PreviewKeyDown` - обработка Tab, Enter, Escape
- `CellEditEnding` - завершение редактирования ячейки
- `LostFocus` / `PreviewLostKeyboardFocus` - потеря фокуса
- `RowEditEnding` - завершение редактирования строки

Выберите подход, который лучше подходит для вашей архитектуры!
 */