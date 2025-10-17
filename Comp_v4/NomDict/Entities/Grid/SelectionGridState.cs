using System.Windows.Input;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.View;
using Comp_v4.NomDict.Vm;
using Comp.ModelData.Comp;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.NomDict.Entities;

public class SelectionGridState : BaseSGridState, IGridSelectingStateHandler
{
    protected readonly DataGridVm _dataGridVm;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    protected TaskCompletionSource<Component>? _selectionTcs;
    
    public SelectionGridState(DataGridVm dataGridVm, IWindowOrderLocator windowOrderLocator) {
        _dataGridVm = dataGridVm;
        _windowOrderLocator = windowOrderLocator;
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }
    public override void OnMouseDoubleClick(object sender, MouseButtonEventArgs e, Grid context) {
        if (_selectionTcs is null)
            throw new InvalidOperationException("Selection grid state has not been started yet");
        if (_dataGridVm.SelectedItem == null) 
            return;
        _selectionTcs.TrySetResult(_dataGridVm.SelectedItem);
        _selectionTcs = null;
        _ = context.ChangeState(context.GetState<EditGridState>(), context);
    }

    public override void AddComponent(object? parameter) {
        throw new SystemException();
    }

    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
        _selectionTcs?.TrySetCanceled();
    }

    public void OnSelecting(TaskCompletionSource<Component> tcs) {
        _windowOrderLocator.MoveToFront<NomDictWindow>();
        if (_selectionTcs is { Task.IsCompleted: false }) {
            tcs.TrySetCanceled(); 
            return;
        }
        _selectionTcs = tcs;
    }
}