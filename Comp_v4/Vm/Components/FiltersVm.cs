using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v4.Operations.Commands.Filtering;
using Utils.EventBus;
using WPF.Templates.TableWindow.Events;

namespace WPF.Templates.TableWindow.Vm.Components;

public class FiltersVm : ObservableObject
{
    protected string? _filterDesignation;
    protected string? _filterName;
    protected bool _ignoreCase = true;

    public string? FilterDesignation {
        get => _filterDesignation;
        set {
            _filterDesignation = value;
            OnPropertyChanged();
        }
    }

    public string? FilterName {
        get => _filterName;
        set {
            _filterName = value;
            OnPropertyChanged();
        }
    }

    public bool IgnoreCase {
        get => _ignoreCase;
        set {
            _ignoreCase = value;
            OnPropertyChanged();
        }
    }
}