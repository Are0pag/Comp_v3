using CommunityToolkit.Mvvm.Input;
using Comp.ModelData;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;

public partial class ResetOrderDateButVm : BaseButtonAdvanced
{
    protected SupplierOrder _supplierOrder;

    public SupplierOrder SupplierOrder {
        get => _supplierOrder;
        set => _supplierOrder = value;
    }
    
    public ResetOrderDateButVm() {
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
    protected SupplierOrder _supplierOrder;
    
    public SupplierOrder SupplierOrder {
        get => _supplierOrder;
        set => _supplierOrder = value;
    }
    
    public ResetDeliveryDateButVm() {
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


