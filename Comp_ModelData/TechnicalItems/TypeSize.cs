using Utils.WPF.Mvvm;

namespace CL_Comp_ModelData.TechnicalItems;

public class TypeSize : NotifyPropertyChanged
{
    protected string _designation;
    protected int _outputsNumber;
    protected bool _isUsingSmd;
    protected string _imagePath;
    protected string _description;
    
    public int Id { get; set; }

    public string Designation {
        get => _designation;
        set {
            if (value == _designation) return;
            _designation = value;
            OnPropertyChanged();
        }
    }

    public int OutputsNumber {
        get => _outputsNumber;
        set {
            if (value == _outputsNumber) return;
            _outputsNumber = value;
            OnPropertyChanged();
        }
    }

    public bool IsUsingSmd {
        get => _isUsingSmd;
        set {
            if (value == _isUsingSmd) return;
            _isUsingSmd = value;
            OnPropertyChanged();
        }
    }

    public string ImagePath {
        get => _imagePath;
        set {
            if (value == _imagePath) return;
            _imagePath = value;
            OnPropertyChanged();
        }
    }

    public string Description {
        get => _description;
        set {
            if (value == _description) return;
            _description = value;
            OnPropertyChanged();
        }
    }
}