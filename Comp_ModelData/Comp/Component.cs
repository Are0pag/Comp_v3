using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp;

[Table(nameof(Component) + "s")]
public class Component : NotifyPropertyChanged 
{
    protected string _name = string.Empty;
    protected string _nomenclatureNumber = string.Empty;
    protected Category _category;
    /*protected ConditionalDesignation _conditionalDesignation;
    protected Manufacturer _manufacturer;
    protected MeasurementUnit _measurementUnit;
    protected TypeSize _typeSize;*/

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Foreign key свойства
    public int CategoryId { get; set; }
    /*public int ConditionalDesignationId { get; set; }
    public int ManufacturerId { get; set; }
    public int MeasurementUnitId { get; set; }
    public int TypeSizeId { get; set; }*/

    
    // Навигационные свойства
    [ForeignKey(nameof(CategoryId))]
    public Category Category {
        get => _category;
        set {
            if (_category == value) return;
            _category = value;
            //CategoryId = value?.Id ?? 0;
            OnPropertyChanged();
        }
    }
    
    /*[ForeignKey(nameof(ConditionalDesignationId))]
    public ConditionalDesignation ConditionalDesignation { 
        get => _conditionalDesignation;
        set {
            _conditionalDesignation = value; 
            ConditionalDesignationId = value?.Id ?? 0;
            OnPropertyChanged();
        }
    }

    [ForeignKey(nameof(ManufacturerId))]
    public Manufacturer Manufacturer { 
        get => _manufacturer;
        set {
            _manufacturer = value; 
            ManufacturerId = value?.Id ?? 0;
            OnPropertyChanged();
        }
    }

    [ForeignKey(nameof(MeasurementUnitId))]
    public MeasurementUnit MeasurementUnit { 
        get => _measurementUnit;
        set {
            _measurementUnit = value; 
            MeasurementUnitId = value?.Id ?? 0;
            OnPropertyChanged();
        }
    }

    [ForeignKey(nameof(TypeSizeId))]
    public TypeSize TypeSize { 
        get => _typeSize;
        set {
            _typeSize = value; 
            TypeSizeId = value?.Id ?? 0;
            OnPropertyChanged();
        }
    }*/
    
    [Required]
    public string Name {
        get => _name;
        set {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }
    
    [Required]
    public string NomenclatureNumber {
        get => _nomenclatureNumber;
        set {
            if (value == _nomenclatureNumber) return;
            _nomenclatureNumber = value;
            OnPropertyChanged();
        }
    }
}