using System.Windows.Input;
using Comp_v4.NomDict.Events;
using Comp_v4.NomDict.View;
using Comp_v4.NomDict.Vm;
using Comp.ModelData.Comp;
using Microsoft.Extensions.DependencyInjection;
using Utils.EventBus;
using Utils.WPF;

namespace Comp_v4.NomDict.Entities;

public class SelectionGridState : BaseSGridState, IGridSelectingStateHandler, ICommitSelectionHandler
{
    protected readonly DataGridVm _dataGridVm;
    protected readonly IWindowOrderLocator _windowOrderLocator;
    protected readonly IServiceProvider _serviceProvider;
    protected TaskCompletionSource<Component>? _selectionTcs;
    
    public SelectionGridState(DataGridVm dataGridVm, IWindowOrderLocator windowOrderLocator, IServiceProvider serviceProvider) {
        _dataGridVm = dataGridVm;
        _windowOrderLocator = windowOrderLocator;
        _serviceProvider = serviceProvider;
        EventBus<INomDictWindowSubscriber>.Subscribe(this);
    }


    public void Dispose() {
        EventBus<INomDictWindowSubscriber>.Unsubscribe(this);
        _selectionTcs?.TrySetCanceled();
    }

    public async Task OnCommitSelection(TaskCompletionSource<Component> tcs) {
        if (_selectionTcs is null)
            throw new InvalidOperationException("Selection grid state has not been started yet");
        if (_dataGridVm.SelectedItem == null) 
            return;
        _selectionTcs.SetResult(_dataGridVm.SelectedItem);
        var grid = _serviceProvider.GetRequiredService<Grid>();
        await grid.ChangeState(grid.GetState<EditGridState>(), grid);
        
        EventBus<INomDictWindowSubscriber>
           .RaiseEvent<IGetResultOfSelectionHanlder>(h => {
                h?.OnGetResultOfSelection(_dataGridVm.SelectedItem);
            });
    }

    void IGridSelectingStateHandler.OnSelecting(TaskCompletionSource<Component> tcs) {
        _windowOrderLocator.MoveToFront<NomDictWindow>();
        _selectionTcs = tcs;
    }

    public override async Task OnMouseDoubleClick(TaskCompletionSource tcs, object sender, MouseButtonEventArgs mouseButtonEventArgs, Grid grid) {

    }

    public override async Task Add(TaskCompletionSource tcs, object? parameter, Grid grid) {
        throw new NotSupportedException();
    }

    public override async Task EditComp(TaskCompletionSource tcs, object? parameter, Grid grid) {
        throw new NotSupportedException();
    }
}