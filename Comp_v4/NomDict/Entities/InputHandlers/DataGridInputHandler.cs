using System.Windows.Input;
using Templates.Common.Events.Input;
using Utils.EventBus;

namespace Comp_v4.NomDict.Entities.InputHandlers;

public class DataGridInputHandler : IMouseDoubleClickHandler
{
    protected readonly Action<object?> _targetAction;
    public DataGridInputHandler(Action<object?> targetAction) {
        _targetAction = targetAction;
        EventBus<IGlobalMouseSubscriber>.Subscribe(this);
    }
    public void Dispose() {
        EventBus<IGlobalMouseSubscriber>.Unsubscribe(this);
    }

    public void OnMouseDoubleClick(object sender, MouseButtonEventArgs e) {
        _targetAction?.Invoke(e);
    }
}