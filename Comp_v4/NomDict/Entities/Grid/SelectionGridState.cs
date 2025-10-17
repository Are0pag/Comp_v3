using System.Windows.Input;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.Vm;
using Comp.ModelData.Comp;
using Utils.EventBus;

namespace Comp_v4.NomDict.Entities;

public class SelectionGridState : BaseSGridState, IGridSelectingStateHandler
{
    protected readonly DataGridVm _dataGridVm;
    private readonly object _lockObject = new object();
    protected TaskCompletionSource<Component>? _selectionTcs;
    
    public SelectionGridState(DataGridVm dataGridVm) {
        _dataGridVm = dataGridVm;
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
        if (_selectionTcs is { Task.IsCompleted: false }) {
            tcs.TrySetCanceled(); 
            return;
        }
        _selectionTcs = tcs;
    }
}