using CommunityToolkit.Mvvm.ComponentModel;

namespace WPF.Templates.TableWindow.Vm.Components;

public abstract class FiltersVmBase : ObservableObject
{
    protected bool _ignoreCase = true;
    public bool IgnoreCase {
        get => _ignoreCase;
        set {
            _ignoreCase = value;
            OnPropertyChanged();
        }
    }
}

public abstract class FiltersVmBaseWithName : FiltersVmBase
{
    protected string? _filterName;
    public string? FilterName {
        get => _filterName;
        set {
            _filterName = value;
            OnPropertyChanged();
        }
    }
}

public class FiltersVm : FiltersVmBaseWithName
{
    protected string? _filterDesignation;

    public string? FilterDesignation {
        get => _filterDesignation;
        set {
            _filterDesignation = value;
            OnPropertyChanged();
        }
    }
}