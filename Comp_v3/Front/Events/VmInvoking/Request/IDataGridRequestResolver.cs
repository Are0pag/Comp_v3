using System.Windows;

namespace Comp_v3.Front.Events.VmInvoking.Request;

/* Такое полезно для объектов, зарезанных как Transient */

public interface IDataGridRequestResolver<T> : IVmGlobalSubscriber
    where T : Window
{
    void GetGrid(IDataGridRequester<T> requester);
}