using Utils.WPF.Mvvm;

namespace CL_Comp_ModelData.Comp.StringParams;

public class DescriptiveParams : NotifyPropertyChanged
{
    protected string _componentDescription = "-";
    protected string _comment = "-";
    public string ComponentDescription {
        get => _componentDescription;
        set {
            if (value == _componentDescription)
                return;
            _componentDescription = value;
            OnPropertyChanged();
        }
    }
    public string Comment {
        get => _comment;
        set {
            if (value == _comment)
                return;
            _comment = value;
            OnPropertyChanged();
        }
    }
}
