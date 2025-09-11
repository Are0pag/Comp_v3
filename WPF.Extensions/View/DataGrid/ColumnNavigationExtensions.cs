using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPF.Extensions.View.Elements;

public static class ColumnNavigationExtensions
{
        public static bool MoveToNextEditableCell(this DataGrid dataGrid, DataGridBeginningEditEventArgs currentEditArgs) {
            if (currentEditArgs == null || currentEditArgs.Row == null || currentEditArgs.Column == null)
                return false;

            var currentItem = currentEditArgs.Row.Item;
            var currentColumn = currentEditArgs.Column;

            // Получаем все видимые редактируемые колонки
            var editableColumns = dataGrid.Columns
                .Where(c => c.Visibility == Visibility.Visible && 
                           !c.IsReadOnly &&
                           c is DataGridBoundColumn)
                .ToList();

            if (editableColumns.Count == 0)
                return false;

            var currentIndex = editableColumns.IndexOf(currentColumn);
            
            if (currentIndex < 0) // Если текущая колонка не в списке редактируемых
            {
                // Находим первую редактируемую колонку для текущей строки
                var firstEditableColumn = editableColumns.FirstOrDefault();
                if (firstEditableColumn != null)
                {
                    dataGrid.CurrentCell = new DataGridCellInfo(currentItem, firstEditableColumn);
                    dataGrid.BeginEdit();
                    return true;
                }
                return false;
            }

            // Определяем следующую колонку
            DataGridColumn nextColumn;
            object nextItem = currentItem;
            
            if (currentIndex < editableColumns.Count - 1)
            {
                nextColumn = editableColumns[currentIndex + 1];
            }
            else
            {
                // Если это последняя колонка, переходим к следующей строке
                var items = dataGrid.ItemsSource as System.Collections.IList;
                if (items == null) return false;

                var currentItemIndex = items.IndexOf(currentItem);
                if (currentItemIndex < items.Count - 1)
                {
                    nextColumn = editableColumns[0];
                    nextItem = items[currentItemIndex + 1];
                }
                else
                {
                    return false; // Достигнут конец таблицы
                }
            }

            // Устанавливаем новую ячейку и начинаем редактирование
            dataGrid.CurrentCell = new DataGridCellInfo(nextItem, nextColumn);
            dataGrid.ScrollIntoView(nextItem, nextColumn);
            dataGrid.BeginEdit();
            
            return true;
        }}