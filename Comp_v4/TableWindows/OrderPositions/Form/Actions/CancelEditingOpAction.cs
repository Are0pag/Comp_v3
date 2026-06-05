using Comp_v4.TableWindows.OrderPositions.Events;
using Comp_v4.TableWindows.OrderPositions.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.OrderPositions.Form.Actions;

public class CancelEditingOpAction : BaseActionAsyncSelfWaiting, IStartEditingOpHandler
{
    /// <summary>
    /// экземпляр который крутится в системе
    /// </summary>
    protected OrderPosition _current;
    
    /// <summary>
    /// созранённый исходные значения
    /// </summary>
    protected OrderPosition _original;
    
    public CancelEditingOpAction(CancelEditingOpButVm button) : base(button) {
        EventBus<IOrderPositionSubscriber>.Subscribe(this);
    }

    public override async Task Perform(TaskCompletionSource tcs) {
        _current.PopulateFrom(_original);

        if (new InstanceContainer<OrderPositionForm>().RuntimeParam is not { } window) {
            Console.Error.WriteLine("Invalid window param");
            throw new ApplicationException("Invalid window param");
        }
        window.Close();
    }

    public void OnStartEditing(object? args = null) {
        if (args is not OrderPosition op) {
            Console.Error.WriteLine("OnStartEditing is called with invalid argument");
            throw new InvalidCastException();
        }
            
        _current = op;
        _original = new OrderPosition().PopulateFrom(op);
    }

    public void Dispose() {
        EventBus<IOrderPositionSubscriber>.Unsubscribe(this);
    }
}