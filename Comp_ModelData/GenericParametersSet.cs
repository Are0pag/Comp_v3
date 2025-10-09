using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Comp.ModelData.TechnicalItems;

[Table(nameof(GenericParametersSet) + "s")]
public class GenericParametersSet : ObservableObject, IDbEntity
{
    protected string _name = string.Empty;
    protected string _gpMain = string.Empty;
    protected string _gp1 = string.Empty;
    protected string _gp2 = string.Empty;
    protected string _gp3 = string.Empty;
    protected string _gp4 = string.Empty;
    protected string _gp5  = string.Empty;
    
    public GenericParametersSet() {
        
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Name {
        get => _name;
        set {
            if (_name == value) return;
            _name = value;
            OnPropertyChanged();
        }
    }

    public string GpMain {
        get => _gpMain;
        set {
            if (_gpMain == value) return;
            _gpMain = value;
            OnPropertyChanged();
        }
    }

    public string Gp1 {
        get => _gp1;
        set {
            if (_gp1 == value) return;
            _gp1 = value;
            OnPropertyChanged();
        }
    }

    public string Gp2 {
        get => _gp2;
        set {
            if (_gp2 == value) return;
            _gp2 = value;
            OnPropertyChanged();
        }
    }

    public string Gp3 {
        get => _gp3;
        set {
            if (_gp3 == value) return;
            _gp3 = value;
            OnPropertyChanged();
        }
    }

    public string Gp4 {
        get => _gp4;
        set {
            if (_gp4 == value) return;
            _gp4 = value;
            OnPropertyChanged();
        }
    }

    public string Gp5 {
        get => _gp5;
        set {
            if (_gp5 == value) return;
            _gp5 = value;
            OnPropertyChanged();
        }
    }
}