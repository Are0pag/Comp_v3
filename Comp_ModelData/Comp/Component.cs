using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp;

[Table(nameof(Component) + "s")]
public class Component : NotifyPropertyChanged, IDbEntity
{
    protected string _name = string.Empty;
    protected string _nomenclatureNumber = string.Empty;
    protected string _catalogNumber = string.Empty;
    protected string _labelingOptions = string.Empty;
    protected string _codeOfElement = string.Empty;
    
    protected string _url = string.Empty;
    protected string _urlAlternative = string.Empty;
    protected string _filePath = string.Empty;
    
    protected string _qrCodeData = string.Empty;
    protected string _description = string.Empty;
    protected string _comments = string.Empty;
    
    protected string? _imagePath;
    
    /// <summary>
    /// GP - Generic Parameters. Model containing values 
    /// </summary>
    protected string _gpMain = string.Empty;
    protected string _gp1 = string.Empty;
    protected string _gp2 = string.Empty;
    protected string _gp3 = string.Empty;
    protected string _gp4 = string.Empty;
    protected string _gp5 = string.Empty;
    
    protected Category _category;
    protected GenericParametersSet? _genericParametersSet;
    protected ConditionalDesignation? _conditionalDesignation;
    protected Manufacturer? _manufacturer;
    protected MeasurementUnit? _measurementUnit;
    protected TypeSize? _typeSize;
    

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Foreign key свойства
    public int CategoryId { get; set; }
    public int? GenericParametersSetId { get; set; }
    public int? ConditionalDesignationId { get; set; }
    public int? ManufacturerId { get; set; }
    public int? MeasurementUnitId { get; set; }
    public int? TypeSizeId { get; set; }

    
    // Навигационные свойства
    [ForeignKey(nameof(CategoryId))]
    public Category Category {
        get => _category;
        set {
            if (_category == value) return;
            _category = value;
            OnPropertyChanged();
        }
    }

    [ForeignKey(nameof(GenericParametersSetId))]
    public GenericParametersSet? GenericParametersSet {
        get => _genericParametersSet;
        set {
            if (_genericParametersSet == value) return;
            _genericParametersSet = value;
            OnPropertyChanged();
        }
    }
    
    [ForeignKey(nameof(ConditionalDesignationId))]
    public ConditionalDesignation? ConditionalDesignation { 
        get => _conditionalDesignation;
        set {
            _conditionalDesignation = value; 
            OnPropertyChanged();
        }
    }

    [ForeignKey(nameof(ManufacturerId))]
    public Manufacturer? Manufacturer { 
        get => _manufacturer;
        set {
            _manufacturer = value; 
            OnPropertyChanged();
        }
    }

    [ForeignKey(nameof(MeasurementUnitId))]
    public MeasurementUnit? MeasurementUnit { 
        get => _measurementUnit;
        set {
            _measurementUnit = value; 
            OnPropertyChanged();
        }
    }

    [ForeignKey(nameof(TypeSizeId))]
    public TypeSize? TypeSize { 
        get => _typeSize;
        set {
            _typeSize = value; 
            OnPropertyChanged();
        }
    }
    
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
    
    

    public string CatalogNumber {
        get => _catalogNumber;
        set {
            if (value == _catalogNumber) return;
            _catalogNumber = value;
            OnPropertyChanged();
        }
    }

    public string LabelingOptions {
        get => _labelingOptions;
        set {
            if (value == _labelingOptions) return;
            _labelingOptions = value;
            OnPropertyChanged();
        }
    }

    public string CodeOfElement {
        get => _codeOfElement;
        set {
            if (value == _codeOfElement) return;
            _codeOfElement = value;
            OnPropertyChanged();
        }
    }


#region Links

    public string Url {
        get => _url;
        set {
            if (value == _url) return;
            _url = value;
            OnPropertyChanged();
        }
    }

    public string UrlAlternative {
        get => _urlAlternative;
        set {
            if (value == _urlAlternative) return;
            _urlAlternative = value;
            OnPropertyChanged();
        }
    }

    public string FilePath {
        get => _filePath;
        set {
            if (value == _filePath) return;
            _filePath = value;
            OnPropertyChanged();
        }
    }
    
    public string? ImagePath {
        get => _imagePath;
        set {
            if (value == _imagePath) return;
            _imagePath = value;
            OnPropertyChanged();
        }
    }

#endregion
    

    public string QrCodeData {
        get => _qrCodeData;
        set {
            if (value == _qrCodeData) return;
            _qrCodeData = value;
            OnPropertyChanged();
        }
    }

    public string Description {
        get => _description;
        set {
            if (value == _description) return;
            _description = value;
            OnPropertyChanged();
        }
    }

    public string Comments {
        get => _comments;
        set {
            if (value == _comments) return;
            _comments = value;
            OnPropertyChanged();
        }
    }

#region GenericParamsValues

    public string GpMain {
        get => _gpMain;
        set {
            if (value == _gpMain) return;
            _gpMain = value;
            OnPropertyChanged();
        }
    }

    public string Gp1 {
        get => _gp1;
        set {
            if (value == _gp1) return;
            _gp1 = value;
            OnPropertyChanged();
        }
    }

    public string Gp2 {
        get => _gp2;
        set {
            if (value == _gp2) return;
            _gp2 = value;
            OnPropertyChanged();
        }
    }

    public string Gp3 {
        get => _gp3;
        set {
            if (value == _gp3) return;
            _gp3 = value;
            OnPropertyChanged();
        }
    }

    public string Gp4 {
        get => _gp4;
        set {
            if (value == _gp4) return;
            _gp4 = value;
            OnPropertyChanged();
        }
    }

    public string Gp5 {
        get => _gp5;
        set {
            if (value == _gp5) return;
            _gp5 = value;
            OnPropertyChanged();
        }
    }

#endregion
}