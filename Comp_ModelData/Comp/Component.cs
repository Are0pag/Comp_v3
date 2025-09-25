using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Comp.ModelData.TechnicalItems;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp;

[Table(nameof(Component) + "s")]
public class Component : NotifyPropertyChanged 
{
    private ConditionalDesignation _conditionalDesignation;
    private Manufacturer _manufacturer;
    private MeasurementUnit _measurementUnit;
    private TypeSize _typeSize;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Foreign key свойства
    public int ConditionalDesignationId { get; set; }
    public int ManufacturerId { get; set; }
    public int MeasurementUnitId { get; set; }
    public int TypeSizeId { get; set; }

    // Навигационные свойства
    [ForeignKey(nameof(ConditionalDesignationId))]
    public ConditionalDesignation ConditionalDesignation { 
        get => _conditionalDesignation;
        set { _conditionalDesignation = value; OnPropertyChanged(); }
    }

    [ForeignKey(nameof(ManufacturerId))]
    public Manufacturer Manufacturer { 
        get => _manufacturer;
        set { _manufacturer = value; OnPropertyChanged(); }
    }

    [ForeignKey(nameof(MeasurementUnitId))]
    public MeasurementUnit MeasurementUnit { 
        get => _measurementUnit;
        set { _measurementUnit = value; OnPropertyChanged(); }
    }

    [ForeignKey(nameof(TypeSizeId))]
    public TypeSize TypeSize { 
        get => _typeSize;
        set { _typeSize = value; OnPropertyChanged(); }
    }
}