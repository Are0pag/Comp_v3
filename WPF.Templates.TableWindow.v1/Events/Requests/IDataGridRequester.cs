using System.Windows;

namespace WPF.Templates.TableWindow.Events.Requests;

public interface IDataGridRequester<T>
    where T : Window
{
    System.Windows.Controls.DataGrid DataGrid { get; set; }
}