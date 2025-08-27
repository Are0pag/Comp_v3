using Comp.ModelData.TechnicalItems;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp.ForeingParams;

public class TechnicalParams : NotifyPropertyChanged
{
    protected MeasurementUnit _measurementUnit;
    protected Manufacturer _manufacturer;
    protected ConditionalDesignation _conditionalDesignation;
    protected TypeSize _typeSize;
    public MeasurementUnit MeasurementUnit {
        get => _measurementUnit;
        set {
            if (value == _measurementUnit)
                return;
            _measurementUnit = value;
            OnPropertyChanged();
        }
    }
    public Manufacturer Manufacturer {
        get => _manufacturer;
        set {
            if (value == _manufacturer)
                return;
            _manufacturer = value;
            OnPropertyChanged();
        }
    }
    public ConditionalDesignation ConditionalDesignation {
        get => _conditionalDesignation;
        set {
            if (value == _conditionalDesignation)
                return;
            _conditionalDesignation = value;
            OnPropertyChanged();
        }
    }
    public TypeSize TypeSize {
        get => _typeSize;
        set {
            if (value == _typeSize)
                return;
            _typeSize = value;
            OnPropertyChanged();
        }
    }
}
