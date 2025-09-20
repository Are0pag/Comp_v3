using System.Windows;
using Comp_v4.Entities;
using Comp.ModelData.TechnicalItems;
using WPF.Templates;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4;

public class DummyWindowEventsHandler<TWindow, T, TFiltersSource> 
    where TWindow : Window
    where T : class, IDbEntity, new()
    where TFiltersSource : FiltersVmBase
{
    protected readonly ActionStackTracker _actionStackTracker;
    protected readonly PersistenceManager<TWindow, T> _persistenceManager;
    protected readonly TableCommandBinder<TWindow, T> _tableCommandBinder;
    protected readonly ActionFilter<TWindow, T, TFiltersSource> _actionFilter;

    public DummyWindowEventsHandler(ActionStackTracker actionStackTracker, 
                               PersistenceManager<TWindow, T> persistenceManager, 
                               TableCommandBinder<TWindow, T> tableCommandBinder, 
                               ActionFilter<TWindow, T, TFiltersSource> actionFilter) {
        _actionStackTracker = actionStackTracker;
        _persistenceManager = persistenceManager;
        _tableCommandBinder = tableCommandBinder;
        _actionFilter = actionFilter;
    }
}