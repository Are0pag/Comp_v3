using System.Windows.Controls;

namespace WPF.Services.View.AutoNavigation.Focusing;

public class Mover
{
    public void MoveToLeftCell(System.Windows.Controls.DataGrid dataGrid) {
        DataGridCellInfo currentCell = dataGrid.CurrentCell;
    
        // Проверяем, что текущая ячейка существует
        if (currentCell.IsValid)
        {
            DataGridColumn currentColumn = currentCell.Column;
            int currentColumnIndex = dataGrid.Columns.IndexOf(currentColumn);
        
            // Проверяем возможность движения влево
            if (currentColumnIndex > 0)
            {
                DataGridColumn leftColumn = dataGrid.Columns[currentColumnIndex - 1];
                object currentItem = currentCell.Item;
            
                // Устанавливаем текущую ячейку слева
                dataGrid.CurrentCell = new DataGridCellInfo(currentItem, leftColumn);
            
                dataGrid.Focus();
                dataGrid.BeginEdit();
            }
        }
    }
}