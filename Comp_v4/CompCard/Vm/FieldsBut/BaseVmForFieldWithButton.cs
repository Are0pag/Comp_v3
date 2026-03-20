
using Comp_v4.CompCard.Events;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;

namespace Comp_v4.CompCard.Vm;

public abstract class BaseVmForFieldWithButton<T> : BaseFieldVm, IExternalTableInputHandler<T>, ICompCardLoadedHandler
    where T : class, IDbEntity, new()
{
    protected readonly Action _openWindow;

    protected BaseVmForFieldWithButton(Action openWindow) {
        _openWindow = openWindow;
        EventBus<ICompCardSubscriber>.Subscribe(this);
    }
    
    public void Dispose() {
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
    }

    public abstract void OnCompCardLoaded(Component component);

    protected abstract void OpenNestedWindow();

    public abstract void HandleTableInput(T? args);
}