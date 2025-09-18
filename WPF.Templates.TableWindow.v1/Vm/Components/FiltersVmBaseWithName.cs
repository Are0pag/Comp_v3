namespace WPF.Templates.TableWindow.Vm.Components;

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