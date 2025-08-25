namespace Utils.WPF;

public class WrapperOfModelProperty<TModelPropertyType> : NotifyPropertyChanged 
{
    private readonly ObservableModelProperty<TModelPropertyType> _modelProperty;
    public WrapperOfModelProperty(ObservableModelProperty<TModelPropertyType> modelProperty) {
        _modelProperty = modelProperty;
        _modelProperty.PropertyChanged += (_, _) => OnPropertyChanged(nameof(Value));
    }

    public TModelPropertyType Value {
        get => _modelProperty.Value;
        set {
            _modelProperty.Value = value;
            OnPropertyChanged();
        }
    }
}