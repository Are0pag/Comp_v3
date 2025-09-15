using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData.TechnicalItems;

namespace WPF.Templates.TableWindow.Vm.Components;

public abstract partial class FilterBase : ObservableObject
{
    protected string _filterText = string.Empty;
    protected bool _isCaseSensitive;

    public string FilterText {
        get => _filterText;
        set {
            if (_filterText == value) return;
            _filterText = value;
            OnPropertyChanged();
        }
    }

    public bool IsCaseSensitive {
        get => _isCaseSensitive;
        set {
            if (_isCaseSensitive == value) return;
            _isCaseSensitive = value;
            OnPropertyChanged();
        }
    }

    public abstract bool Filter(object item);

    protected bool StringContains(string source, string filter) {
        if (string.IsNullOrEmpty(filter)) return true;
        if (string.IsNullOrEmpty(source)) return false;

        var comparison = IsCaseSensitive 
            ? StringComparison.CurrentCulture 
            : StringComparison.CurrentCultureIgnoreCase;

        return source.Contains(filter, comparison);
    }
}

public partial class DesignationFilter : FilterBase 
{
    public override bool Filter(object item) {
        return item is ConditionalDesignation designation && StringContains(designation.Designation, FilterText);
    }
}

public partial class NameFilter : FilterBase
{
    public override bool Filter(object item) {
        return item is ConditionalDesignation designation && StringContains(designation.Name, FilterText);
    }
}