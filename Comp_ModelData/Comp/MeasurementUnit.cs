using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.TechnicalItems;

[Table(nameof(MeasurementUnit) + "s")]
public class MeasurementUnit : NotifyPropertyChanged, IDbEntity
{
    protected string _name;
    protected string _designation;

    public MeasurementUnit() {
        _name = string.Empty;
        _designation = string.Empty;
    }
    public MeasurementUnit(string name, string designation) {
        _name = name;
        _designation = designation;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name {
        get => _name;
        set {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }

    [Required]
    public string Designation {
        get => _designation;
        set {
            if (value == _designation) return;
            _designation = value;
            OnPropertyChanged();
        }
    }
}