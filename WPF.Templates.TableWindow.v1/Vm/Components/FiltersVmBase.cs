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