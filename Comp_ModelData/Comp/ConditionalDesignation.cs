using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.TechnicalItems;

[Table(nameof(ConditionalDesignation) + "s")]
public class ConditionalDesignation : NotifyPropertyChanged, IDbEntity, ICloneable
{
    protected string _name;
    protected string _designation;
    
    public const byte MAX_NAME_LENGTH = 250;
    public const byte MAX_DESIGNATION_LENGTH = 100;

    public ConditionalDesignation() {
        Name = string.Empty;
        Designation = string.Empty;
    }
    public ConditionalDesignation(string name, string designation) {
        _name = name;
        _designation = designation;
    }
        
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [MaxLength(MAX_NAME_LENGTH)]
    public string Name {
        get => _name;
        set {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }
    
    [Required]
    [MaxLength(MAX_DESIGNATION_LENGTH)]
    public string Designation {
        get => _designation;
        set {
            if (value == _designation) return;
            _designation = value;
            OnPropertyChanged();
        }
    }

    public object Clone() => new ConditionalDesignation(Name, Designation);
}