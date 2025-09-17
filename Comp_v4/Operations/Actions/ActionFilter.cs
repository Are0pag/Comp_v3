using System.ComponentModel;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Commands.Filtering;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.States;
using WPF.Templates.TableWindow.Vm.Components;

namespace WPF.Templates;

public class ActionFilter : BaseAction, IFilteringHandler
{
    protected readonly FiltersVm _filtersVm;
    protected readonly Cell _cell;
    protected ApplyFilterCommand? _previousFilterCommand;
    
    public ActionFilter(IModuleCommandScheduler scheduler, ModuleContext context, CommandFactory commandFactory, FiltersVm filtersVm, Cell cell) : base(scheduler, context, commandFactory) {
        _filtersVm = filtersVm;
        _cell = cell;
        _filtersVm.PropertyChanged += filtersVmOnPropertyChanged();
        EventBus<IGlobSubscriber>.Subscribe(this);
    }

    public override async Task<BaseAction> PerformAsync(object? parameter = null) {
        if (_previousFilterCommand != null) {
            await _previousFilterCommand.UndoAsync();
        }
        
        var arg = new ApplyFilterCommand.Args(_context.DataGridViewModel.Items!, _filtersVm);
        var filterCommand = _commandFactory.CreateCommand<ApplyFilterCommand, ApplyFilterCommand.Args>(arg);
        await filterCommand.ExecuteAsync();
        _previousFilterCommand = filterCommand;
        return this;
    }

    public override bool CanPerform() {
        return _cell.CurrentState is CellStateIdle;
    }

    public override Task CancelAsync(object? parameter = null) {
        return Task.CompletedTask;
    }

    public void Dispose() {
        _filtersVm.PropertyChanged -= filtersVmOnPropertyChanged();
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    object? IFilteringHandler.OnSourceCollectionStartEditing() {
        _filtersVm.FilterName = null;
        _filtersVm.FilterDesignation = null;
        _filtersVm.PropertyChanged -= filtersVmOnPropertyChanged();
        return null;
    }

    object? IFilteringHandler.OnSourceCollectionStopEditing() {
        _filtersVm.PropertyChanged += filtersVmOnPropertyChanged();
        PerformAsync();
        return null;
    }

    protected PropertyChangedEventHandler? filtersVmOnPropertyChanged() {
        return async (_, __) => {
            await PerformAsync();
        };
    }
}