using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.TechnicalItems;

[Table(nameof(TypeSize) + "s")]
public class TypeSize : NotifyPropertyChanged, IDbEntity, IImageOwner
{
    protected string _designation = string.Empty;
    protected int _outputsNumber;
    protected bool _isUsingSmd;
    protected string _imagePath = string.Empty;
    protected string _description = string.Empty;

    public TypeSize() { }
    
    [Key] // Primary key
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Autoincrement
    public int Id { get; set; }

    [Required]
    public string Designation {
        get => _designation;
        set {
            if (value == _designation) return;
            _designation = value;
            OnPropertyChanged();
        }
    }

    public int OutputsNumber {
        get => _outputsNumber;
        set {
            if (value == _outputsNumber) return;
            _outputsNumber = value;
            OnPropertyChanged();
        }
    }

    public bool IsUsingSmd {
        get => _isUsingSmd;
        set {
            if (value == _isUsingSmd) return;
            _isUsingSmd = value;
            OnPropertyChanged();
        }
    }

    public string ImagePath {
        get => _imagePath;
        set {
            if (value == _imagePath) return;
            _imagePath = value;
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
}