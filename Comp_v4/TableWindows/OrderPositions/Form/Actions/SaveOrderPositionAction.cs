using Comp_v4._Installers;
using Comp_v4.TableWindows.OrderPositions.Form.Entities;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Form.Actions;

public class SaveOrderPositionAction : BaseActionAsyncSelfWaiting, IRuntimeParamsContainer<OrderPosition>
{
    protected readonly OrderPositionValidator _orderPositionValidator;
    protected readonly OpForm _opForm;
    public SaveOrderPositionAction(SaveOrderPositionButVm button, OrderPositionValidator orderPositionValidator, OpForm opForm) : base(button) {
        _orderPositionValidator = orderPositionValidator;
        _opForm = opForm;
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        await _opForm.Save(tcs, RuntimeParam);
        await tcs.Task;
    }

    public override bool CanPerform() {
        return base.CanPerform() && _orderPositionValidator.ValidateAsync(RuntimeParam).Result is { IsValid: true };
    }
    
    protected OrderPosition _orderPosition;
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