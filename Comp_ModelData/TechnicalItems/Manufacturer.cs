using Utils.WPF.Mvvm;

namespace Comp.ModelData.TechnicalItems;

public class Manufacturer : NotifyPropertyChanged, IDbEntity
{
    protected string _designation;
    protected string _fullName;
    protected string _url;
    protected string _remark;

    public Manufacturer(string designation, string fullName, string url, string remark) {
        _designation = designation;
        _fullName = fullName;
        _url = url;
        _remark = remark;
    }
    
    public int Id { get; set; }

    public string Designation {
        get => _designation;
        set {
            if (value == _designation) return;
            _designation = value;
            OnPropertyChanged();
        }
    }

    public string FullName {
        get => _fullName;
        set {
            if (value == _fullName) return;
            _fullName = value;
            OnPropertyChanged();
        }
    }

    public string Url {
        get => _url;
        set {
            if (value == _url) return;
            _url = value;
            OnPropertyChanged();
        }
    }

    public string Remark {
        get => _remark;
        set {
            if (value == _remark) return;
            _remark = value;
            OnPropertyChanged();
        }
    }
}