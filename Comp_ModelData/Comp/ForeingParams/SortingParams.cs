using Comp.ModelData.SortingItems;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp.ForeingParams;

public class SortingParams : NotifyPropertyChanged
{
    protected Category _category;
    protected ElementStatus _elementStatus;
    public Category Category {
        get => _category;
        set {
            if (value == _category)
                return;
            _category = value;
            OnPropertyChanged();
        }
    }

    public ElementStatus ElementStatus {
        get => _elementStatus;
        set {
            if (value == _elementStatus)
                return;
            _elementStatus = value;
            OnPropertyChanged();
        }
    }
}
