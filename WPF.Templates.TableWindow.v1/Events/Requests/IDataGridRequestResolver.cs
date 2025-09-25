using System.Windows;

namespace WPF.Templates.TableWindow.v1.Events.Requests;

/* Такое полезно для объектов, зарезанных как Transient */
public interface IDataGridRequestResolver<T> : IGlobSubscriber
    where T : Window
{
    void GetGrid(IDataGridRequester<T> requester);
}