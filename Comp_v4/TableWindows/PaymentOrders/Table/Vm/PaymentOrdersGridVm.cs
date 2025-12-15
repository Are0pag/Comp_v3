using Comp.Db.Contracts;
using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Vm;

public class PaymentOrdersGridVm : DataGridViewModel<PaymentOrder>
{
    public PaymentOrdersGridVm(IRepository<PaymentOrder> repository) : base(repository) {
    }
}