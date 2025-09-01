namespace Utils.WPF.Mvvm;

public class ObservableModelProperty<TPropType> : NotifyPropertyChanged 
{
    protected TPropType _value;
    protected List<string>? _linkedPropertiesNames;

    public ObservableModelProperty(params string[] linkedPropertiesNames) { /* props that need to update with this value */
        _linkedPropertiesNames = linkedPropertiesNames?.ToList();
    }
    
    public TPropType Value {
        get => _value;
        set {
            _value = value;
            OnPropertyChanged();
            if (_linkedPropertiesNames == null) return;
            foreach (var name in _linkedPropertiesNames)
                OnPropertyChanged(name);
        }
    }

    public void AddLinkedPropertiesNames(params string[] propertyNames) {
        _linkedPropertiesNames ??= new List<string>();
        _linkedPropertiesNames.AddRange(propertyNames);
    }
}