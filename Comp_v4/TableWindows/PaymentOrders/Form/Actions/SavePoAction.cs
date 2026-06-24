using Comp_v4.TableWindows.PaymentOrders.Form.Entities;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm;
using Comp_v4.TableWindows.PaymentOrders.Table.Vm.Buts;
using Comp_v4.TableWindows.SupplierOrders.Table.Vm;
using Comp.Db.Contracts;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.PaymentOrders.Form.Actions;

public class SavePoAction : BaseActionAsyncSelfWaiting
{
    protected readonly PaymentOrderForm _form;
    protected readonly PaymentOrdersGridVm _gridVm;
    protected readonly PoValidator _validator;
    protected readonly IRepository<SupplierOrder> _supplierOrderRepository;
    protected readonly SoDataGridVm _soDataGridVm;
    
    public SavePoAction(SavePaymentOrderButVm button, PaymentOrderForm form, PaymentOrdersGridVm gridVm, PoValidator validator, IRepository<SupplierOrder> supplierOrderRepository, SoDataGridVm soDataGridVm) : base(button) {
        _form = form;
        _gridVm = gridVm;
        _validator = validator;
        _supplierOrderRepository = supplierOrderRepository;
        _soDataGridVm = soDataGridVm;
    }

    public PaymentOrder Po { get; set; } 

    public override async Task Perform(TaskCompletionSource tcs) {
        await _form.Save(tcs, Po);
        new InstanceContainer<PaymentOrderFormWindow>().RuntimeParam.Close();
        if (_form.CurrentState is CreatePoState)
            _gridVm.AddItem(Po);

        if (_soDataGridVm.Items.First(i => i.Id == Po.Order.Id) is not { } order) {
            throw new ApplicationException($"Order with id: {Po.Order.Id} was not found");
        }
        order.TotalPayment += Po.PaymentAmount;
        await _supplierOrderRepository.UpdateAsync(order);
        
        tcs.TrySetResult();
    }

    public override bool CanPerform() {
        if (Po == null) {
            throw new NullReferenceException("Po cannot be null");
        }

        var result = _validator.ValidateAsync(Po).Result;
        if (!result.IsValid)
            return false;
        
        return base.CanPerform();
    }
}