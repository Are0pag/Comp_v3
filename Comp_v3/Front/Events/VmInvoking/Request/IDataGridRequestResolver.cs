namespace Comp_v3.Front.Events.VmInvoking.Request;

/* Такое полезно для объектов, зарезанных как Transient */

public interface IDataGridRequestResolver : IVmGlobalSubscriber
{
    void GetGrid(IDataGridRequester requester);
}