using Comp.Db.Contracts;
using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm;

public class DataGridVm : DataGridViewModel<SupplierOrder>
{
    public DataGridVm(IRepository<SupplierOrder> repository) : base(repository) {
    }
}