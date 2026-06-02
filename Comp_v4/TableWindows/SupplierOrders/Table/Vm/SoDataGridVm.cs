using System.Collections.ObjectModel;
using System.ComponentModel;
using Comp.Db.Contracts;
using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm;

public class SoDataGridVm : FilterDataGridVm<SupplierOrder>
{
    public SoDataGridVm(IRepository<SupplierOrder> repository, FiltersVmBase filtersVm, IFilter<SupplierOrder, FiltersVmBase> filter) 
        : base(repository, filtersVm, filter) {
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