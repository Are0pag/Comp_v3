namespace Utils.WPF.Mvvm;

public class ObservableModelProperty<TPropType> : NotifyPropertyChanged 
{
    private TPropType _value;
    public TPropType Value {
        get => _value;
        set {
            _value = value;
            OnPropertyChanged();
        }
    }
}