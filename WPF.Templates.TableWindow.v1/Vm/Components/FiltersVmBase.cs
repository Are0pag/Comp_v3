using CommunityToolkit.Mvvm.ComponentModel;

namespace WPF.Templates.TableWindow.Vm.Components;

public class FiltersVmBase : ObservableObject
{
    protected bool _ignoreCase = true;
    protected string? _filterString;
    
    public bool IgnoreCase {
        get => _ignoreCase;
        set {
            _ignoreCase = value;
            OnPropertyChanged();
        }
    }

    public string? FilterString {
        get => _filterString;
        set {
            if (_filterString == value) return;
            _filterString = value;
            OnPropertyChanged();
        }
    }
}