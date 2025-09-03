namespace Comp_v3.Front.Events.VmInvoking.Request;

public interface IDataGridRequestResolver : IVmGlobalSubscriber
{
    void GetGrid(IDataGridRequester requester);
}

public interface IDataGridRequester
{
    System.Windows.Controls.DataGrid DataGrid { get; set; }
}