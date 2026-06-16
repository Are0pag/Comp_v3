using System.Collections.ObjectModel;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp.Db.Contracts;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.TableWindows.PaymentOrders.Table.Vm;

public class PaymentOrdersGridVm : FilterGridVm<PaymentOrder>
{
    protected SupplierOrder? _correspondingSo;
    public PaymentOrdersGridVm(IRepository<PaymentOrder> repository, IFilter<PaymentOrder, FiltersVmBase> filter) : base(repository, filter) {
    }
    
    public SoDataGridVm? SoDataGridVm { get; set; }

    public override async Task InitFilteringCollection() {
        if (SoDataGridVm is null)
            throw new NullReferenceException("Доигрался со scope-ами, мудила: SoDataGridVm is null");

        if (SoDataGridVm.LastSelectedSupplierOrder is null)
            throw new Exception();
        
        _correspondingSo = SoDataGridVm.LastSelectedSupplierOrder;
        var data = await _repository.GetAllBySupplierOrderAsync(_correspondingSo.Id);
        Items = new ObservableCollection<PaymentOrder>(data);
        ItemsSorted = new ObservableCollection<PaymentOrder>(Items);
        
        OnPropertyChanged(nameof(ItemsSorted));
    }
}