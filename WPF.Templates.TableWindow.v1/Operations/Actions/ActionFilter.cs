using System.ComponentModel;
using System.Windows;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Commands.Filtering;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;
using WPF.Templates.TableWindow.States;
using WPF.Templates.TableWindow.Vm.Components;

namespace WPF.Templates;

public class ActionFilter<TWindow, T, TFiltersSource> : BaseAction<TWindow, T>, IFilteringHandler
    where TWindow : Window
    where T : class, IDbEntity
    where TFiltersSource : FiltersVmBase
{
    protected readonly TFiltersSource _filtersVm;
    protected readonly Cell<TWindow, T> _cell;
    protected ApplyFilterCommand<TWindow, T, TFiltersSource>? _previousFilterCommand;
    
    public ActionFilter(IDataGridCommandScheduler scheduler, ModuleContext<TWindow, T> context, ICommandFactory commandFactory, TFiltersSource filtersVm, Cell<TWindow, T> cell) 
        : base(scheduler, context, commandFactory) {
        _filtersVm = filtersVm;
        _cell = cell;
        _filtersVm.PropertyChanged += filtersVmOnPropertyChanged();
        EventBus<IGlobSubscriber>.Subscribe(this);
    }

    public override async Task<BaseAction<TWindow, T>> PerformAsync(object? parameter = null) {
        if (_previousFilterCommand != null) {
            await _previousFilterCommand.UndoAsync();
        }
        
        var arg = new ApplyFilterCommand<TWindow, T, TFiltersSource>.Args(_context.DataGridViewModel.Items!, _filtersVm);
        var filterCommand = _commandFactory.CreateCommand<ApplyFilterCommand<TWindow, T, TFiltersSource>, ApplyFilterCommand<TWindow, T, TFiltersSource>.Args>(arg);
        await filterCommand.ExecuteAsync();
        _previousFilterCommand = filterCommand;
        return this;
    }

    public override bool CanPerform() {
        return _cell.CurrentState is CellStateIdle<TWindow, T>;
    }

    public override Task CancelAsync(object? parameter = null) {
        return Task.CompletedTask;
    }

    public void Dispose() {
        _filtersVm.PropertyChanged -= filtersVmOnPropertyChanged();
        EventBus<IGlobSubscriber>.Unsubscribe(this);
    }

    object? IFilteringHandler.OnSourceCollectionStartEditing() {
        ObjectStringCleaner.SetStringPropertiesToNull(_filtersVm);
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