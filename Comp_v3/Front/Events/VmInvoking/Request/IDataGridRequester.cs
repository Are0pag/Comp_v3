using System.Windows;

namespace Comp_v3.Front.Events.VmInvoking.Request;

public interface IDataGridRequester<T>
    where T : Window
{
    System.Windows.Controls.DataGrid DataGrid { get; set; }
}