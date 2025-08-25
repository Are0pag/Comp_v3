using Utils.WPF;

namespace CL_Comp_ModelData.Comp.StringParams;

public class ExternalParams : NotifyPropertyChanged
{
    protected string _imagePath = "-";
    public string ImagePath {
        get => _imagePath;
        set {
            if (value == _imagePath)
                return;
            _imagePath = value;
            OnPropertyChanged();
        }
    }
}
