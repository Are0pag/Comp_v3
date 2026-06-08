using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm;

public class VatStatusEnumVm : EnumVmSourceChanging<VatStatus, SupplierOrder>, ICreateSupplierOrdersHandler
{
    public VatStatusEnumVm(SupplierOrder source) : base(source) {
        _selectedValue = VatStatus.VatIncluded;
        EventBus<ISupplierOrdersSubscriber>.Subscribe(this);
    }

    public Action OnVatStatusChanged { get; set; }
    public Action OnResetVatStatus { get; set; }
    public override VatStatus SelectedValue {
        get => _selectedValue;
        set {
            SetProperty(ref _selectedValue, value);
            _source.VatStatus = value.ToString();

            // Установка в "Без НДС"
            if (value == VatStatus.WithoutVat) {
                //_source.VatPercentage = 0;
                OnResetVatStatus?.Invoke();
            }
            else { 
                // Если это было переназначение с WithoutVat 
                if (_source.VatPercentage == 0) {
                    //_source.VatPercentage = SupplierOrder.VAT_PERCENTAGE_DEFAULT;
                    OnResetVatStatus?.Invoke();
                }
            }
            OnVatStatusChanged?.Invoke();
        }
    }

    public void Dispose() {
        EventBus<ISupplierOrdersSubscriber>.Unsubscribe(this);
    }

    public Task OnCreateSupplierOrder(TaskCompletionSource tcs, object parameter = null) {
        SelectedValue = Enum.Parse<VatStatus>(_source.VatStatus);
        tcs.TrySetResult();
        return Task.CompletedTask;
    }
}