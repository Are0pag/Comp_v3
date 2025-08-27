using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp.GenericParams;

public class GenValues : NotifyPropertyChanged
{
    protected string _genValueMain;
    protected string _genValue0;
    protected string _genValue1;
    protected string _genValue2;
    protected string _genValue3;
    protected string _genValue4;

    public int Id { get; set; }
    public string GenValueMain {
        get => _genValueMain;
        set {
            if (value == _genValueMain)
                return;
            _genValueMain = value;
            OnPropertyChanged();
        }
    }

    public string GenValue0 {
        get => _genValue0;
        set {
            if (value == _genValue0)
                return;
            _genValue0 = value;
            OnPropertyChanged();
        }
    }

    public string GenValue1 {
        get => _genValue1;
        set {
            if (value == _genValue1)
                return;
            _genValue1 = value;
            OnPropertyChanged();
        }
    }

    public string GenValue2 {
        get => _genValue2;
        set {
            if (value == _genValue2)
                return;
            _genValue2 = value;
            OnPropertyChanged();
        }
    }

    public string GenValue3 {
        get => _genValue3;
        set {
            if (value == _genValue3)
                return;
            _genValue3 = value;
            OnPropertyChanged();
        }
    }

    public string GenValue4 {
        get => _genValue4;
        set {
            if (value == _genValue4)
                return;
            _genValue4 = value;
            OnPropertyChanged();
        }
    }
}
