using Utils.WPF.Mvvm;

namespace CL_Comp_ModelData.TechnicalItems;

public class ConditionalDesignation : NotifyPropertyChanged, ICloneable
{
    protected string _name;
    protected string _designation;

    public ConditionalDesignation() {}
    public ConditionalDesignation(string name, string designation) {
        _name = name;
        _designation = designation;
    }
    
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

    public object Clone() => new ConditionalDesignation(Name, Designation);
}