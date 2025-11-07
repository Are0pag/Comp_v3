using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Comp.ModelData;
using Utils.WPF;
using Utils.WPF.Buttons;
using WPF.UCL;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class SetContractLinkAction : SetLinkAction
{
    public SetContractLinkAction(ContractLinkFieldVm button, ValidatorUrl validatorUrl, SupplierOrder supplierOrder) 
        : base(button, validatorUrl, supplierOrder) {
    }

    public override Task Perform(TaskCompletionSource tcs) {
        base.Perform(tcs);
        _supplierOrder.ContractFilePath = _desiredUrl!;
        return Task.CompletedTask;
    }
}

public class SetInvoiceLinkAction : SetLinkAction
{
    public SetInvoiceLinkAction(InvoiceLinkFieldVm button, ValidatorUrl validatorUrl, SupplierOrder supplierOrder) 
        : base(button, validatorUrl, supplierOrder) {
    }
    
    public override Task Perform(TaskCompletionSource tcs) {
        base.Perform(tcs);
        _supplierOrder.InvoiceFilePath = _desiredUrl!;
        return Task.CompletedTask;
    }
}

public class SetLinkAction : BaseActionAsyncSelfWaiting
{
    protected readonly ValidatorUrl _validatorUrl;
    protected readonly SupplierOrder _supplierOrder;
    protected string? _desiredUrl;
    
    public SetLinkAction(LinkFieldVm button, ValidatorUrl validatorUrl, SupplierOrder supplierOrder) : base(button) {
        _validatorUrl = validatorUrl;
        _supplierOrder = supplierOrder;
    }

    public override Task Perform(TaskCompletionSource tcs) {
        string desiredUrl = "";
        var window = new OneValueWindow($"{((LinkFieldVm)_button).FieldTitle}: ", s => {
            desiredUrl = s;
            return _validatorUrl.ValidateAsync(s).Result is { IsValid: true };
        });
        
        WindowLocator.LocateBy(window)
                     .ShowDialog();
        
        ((LinkFieldVm)_button).Url = desiredUrl;
        _desiredUrl = desiredUrl;
        return Task.CompletedTask;
    }
}