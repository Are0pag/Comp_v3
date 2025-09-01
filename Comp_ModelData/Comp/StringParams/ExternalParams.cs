using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp.StringParams;

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
