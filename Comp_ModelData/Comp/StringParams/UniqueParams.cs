using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp.StringParams;

public class UniqueParams : NotifyPropertyChanged
{
    protected string _name;
    protected string _nomenclatureNumber;
    public string Name {
        get => _name;
        set {
            if (value == _name)
                return;
            _name = value;
            OnPropertyChanged();
        }
    }
    public string NomenclatureNumber {
        get => _nomenclatureNumber;
        set {
            if (value == _nomenclatureNumber)
                return;
            _nomenclatureNumber = value;
            OnPropertyChanged();
        }
    }
}
