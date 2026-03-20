using System.Windows;
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp_v4.TableWindows.SupplierOrders.Form.Entities;
using Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;
using Utils.EventBus;
using Utils.WPF.Buttons;
using WPF.Services.UserActionsHandling.InputText;
using WPF.UCL;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Actions;

public class SaveFormAction : BaseActionAsyncCompletion
{
    protected readonly SupplierOrderFormWindow _supplierOrderFormWindow;
    protected readonly SoForm _soForm;
    public SaveFormAction(SaveFormButVm button, SupplierOrderFormWindow supplierOrderFormWindow, SoForm soForm) : base(button) {
        _supplierOrderFormWindow = supplierOrderFormWindow;
        _soForm = soForm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        try {
            await _soForm.OnCreateSupplierOrder(tcs);
            await tcs.Task;
            _supplierOrderFormWindow.Close();
        }
        catch (InvalidInputException) {
            await Task.Run(() => {
                Application.Current.Dispatcher.Invoke(() => {
                    NotificationWindow.Show("Необходимо заполнить обязательные поля");
                });
            });
        }
    }

    public override bool CanPerform() {
        return true;
    }
}