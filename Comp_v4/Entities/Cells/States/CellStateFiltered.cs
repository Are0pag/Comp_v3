using System.ComponentModel;
using Comp_v4.Entities;
using WPF.Templates.TableWindow.Vm.Components;

namespace WPF.Templates.TableWindow.States;

public class CellStateFiltered : BaseCellStateInput, IDisposable
{
    protected readonly FiltersVm _filtersVm;
    public CellStateFiltered(IModuleCommandScheduler scheduler, ModuleContext context, Validator validator, FiltersVm filtersVm) : base(scheduler, context, validator) {
        _filtersVm = filtersVm;
    }

    public override Task Enter(Cell context) {
        // _filtersVm.Enable()
        _filtersVm.PropertyChanged += OnFiltersChanged;
        return base.Enter(context);
    }

    public override Task Exit(Cell context) {
        _filtersVm.PropertyChanged -= OnFiltersChanged;
        return base.Exit(context);
    }

    protected virtual void OnFiltersChanged(object? sender, PropertyChangedEventArgs e) {
        var sortedItems = _context.DataGridViewModel.Items.Where(item =>
                                                   (string.IsNullOrEmpty(_filtersVm.FilterDesignation) ||
                                                    item.Designation.Contains(_filtersVm.FilterDesignation)) &&
                                                   (string.IsNullOrEmpty(_filtersVm.FilterName) ||
                                                    item.Name.Contains(_filtersVm.FilterName))
                                              ).ToList();
    }

    public virtual void Dispose() {
        _filtersVm.PropertyChanged -= OnFiltersChanged;
    }
}