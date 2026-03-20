using CommunityToolkit.Mvvm.Input;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;

public partial class ResetOrderDateButVm : BaseButtonAdvanced
{
    protected readonly SupplierOrder _supplierOrder;
    
    public ResetOrderDateButVm(SupplierOrder supplierOrder) {
        _supplierOrder = supplierOrder;
        Label = "Сбросить дату";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        _supplierOrder.OrderDate = DateTime.Now;
        return Task.CompletedTask;
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}

public partial class ResetDeliveryDateButVm : BaseButtonAdvanced
{
    protected readonly SupplierOrder _supplierOrder;
    
    public ResetDeliveryDateButVm(SupplierOrder supplierOrder) {
        _supplierOrder = supplierOrder;
        Label = "Сбросить дату";
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public override Task OnClickAsync() {
        _supplierOrder.DeliveryDate = DateTime.Now;
        return Task.CompletedTask;
    }

    public override void NotifyCanExecute() {
        ClickCommand.NotifyCanExecuteChanged();
    }
}


