namespace WPF.Templates.TableWindow.Vm.Components;

public class FiltersVmCd : FiltersVmBaseWithName
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