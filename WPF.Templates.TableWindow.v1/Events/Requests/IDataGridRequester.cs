using System.Windows;

namespace WPF.Templates.TableWindow.v1.Events.Requests;

public interface IDataGridRequester<T>
    where T : Window
{
    System.Windows.Controls.DataGrid DataGrid { get; set; }
}