using Utils.WPF.Mvvm;

namespace Comp.ModelData.TechnicalItems;

public class MeasurementUnit : NotifyPropertyChanged
{
    protected string _name;
    protected string _designation;
    
    public int Id { get; set; }

    public string Name {
        get => _name;
        set {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }

    public string Designation {
        get => _designation;
        set {
            if (value == _designation) return;
            _designation = value;
            OnPropertyChanged();
        }
    }
}