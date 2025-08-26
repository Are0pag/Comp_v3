using Utils.WPF;

namespace CL_Comp_ModelData.Comp.GenericParams;

public class GenKeys : NotifyPropertyChanged
{
    private string _alias;
    private string _mainParameterGenericKey;
    private string _additionalParameterGenericKey0;
    private string _additionalParameterGenericKey1;
    private string _additionalParameterGenericKey2;
    private string _additionalParameterGenericKey3;
    private string _additionalParameterGenericKey4;

    public int Id { get; set; } 
        
    public string Alias {
        get => _alias;
        set {
            if (_alias != value) {
                _alias = value;
                OnPropertyChanged();
            }
        }
    }

    public string MainParameterGenericKey {
        get => _mainParameterGenericKey;
        set {
            if (_mainParameterGenericKey != value) {
                _mainParameterGenericKey = value;
                OnPropertyChanged();
            }
        }
    }
    public string AdditionalParameterGenericKey0 {
        get => _additionalParameterGenericKey0;
        set {
            if (_additionalParameterGenericKey0 != value) {
                _additionalParameterGenericKey0 = value;
                OnPropertyChanged();
            }
        }
    }
    public string AdditionalParameterGenericKey1 {
        get => _additionalParameterGenericKey1;
        set {
            if (_additionalParameterGenericKey1 != value) {
                _additionalParameterGenericKey1 = value;
                OnPropertyChanged();
            }
        }
    }
    public string AdditionalParameterGenericKey2 {
        get => _additionalParameterGenericKey2;
        set {
            if (_additionalParameterGenericKey2 != value) {
                _additionalParameterGenericKey2 = value;
                OnPropertyChanged();
            }
        }
    }
    public string AdditionalParameterGenericKey3 {
        get => _additionalParameterGenericKey3;
        set {
            if (_additionalParameterGenericKey3 != value) {
                _additionalParameterGenericKey3 = value;
                OnPropertyChanged();
            }
        }
    }
    public string AdditionalParameterGenericKey4 {
        get => _additionalParameterGenericKey4;
        set {
            if (_additionalParameterGenericKey4 != value) {
                _additionalParameterGenericKey4 = value;
                OnPropertyChanged();
            }
        }
    }
}