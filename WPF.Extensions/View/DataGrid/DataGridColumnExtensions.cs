using System.Windows.Controls;
using System.Windows.Data;

namespace WPF.Extensions.View.DataGrid;

public static class DataGridColumnExtensions
{
    public static string GetPropertyName(this DataGridColumn column) {
        if (column is not DataGridBoundColumn boundColumn) return string.Empty;
        var binding = boundColumn.Binding as Binding;
        var result = binding?.Path?.Path ?? string.Empty;
        return result;
    }
}