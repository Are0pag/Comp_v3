using Comp.Db.Contracts;
using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm;

public class SoDataGridVm : DataGridViewModel<SupplierOrder>
{
    public SoDataGridVm(IRepository<SupplierOrder> repository) : base(repository) {
    }
    
    public SupplierOrder? LastSelectedSupplierOrder { get; set; }

    public override SupplierOrder? SelectedItem {
        get => base.SelectedItem;
        set {
            base.SelectedItem = value;
            LastSelectedSupplierOrder = value;
        }
    }
}