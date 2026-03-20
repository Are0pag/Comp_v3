using Comp_v4._Installers;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.TableWindows.OrderPositions.Form.Vm;

public class ReceiveStatusEnumVm : EnumVm<ReceiveStatus>, IRuntimeParamsContainer<OrderPosition>
{
    protected OrderPosition _orderPosition;
    public ReceiveStatusEnumVm() {
        SelectedValue = ReceiveStatus.NotReceived;
    }

    public override ReceiveStatus SelectedValue {
        get => _selectedValue;
        set {
            SetProperty(ref _selectedValue, value);
            try {
                RuntimeParam.ReceiveStatus = value.ToString();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message + $"in {nameof(ReceiveStatusEnumVm)}");
            }
        }
    }
    
    public OrderPosition RuntimeParam {
        get {
            try {
                EventBus<IGlSubscriber>.RaiseEvent<IRuntimeParamsResolver<OrderPosition>>(r => {
                    r.ResolveRuntimeParams(this);
                });
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                throw;
            }
            return _orderPosition;
        }
        set {
            _orderPosition = value;
        }
    }
}