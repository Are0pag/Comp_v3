using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp.GenericParams;

[Table(nameof(GenParamsSet) + "s")]
public class GenParamsSet : NotifyPropertyChanged
{
    protected string _alias;

    public GenParamsSet() { }
    public GenParamsSet(string alias) {
        _alias = alias;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Alias {
        get => _alias;
        set {
            if (_alias == value) return;
            _alias = value;
            OnPropertyChanged();
        }
    }

    // Навигационное свойство для параметров
    public ObservableCollection<GenParamsItem> GenParamsItems { get; set; } = new ObservableCollection<GenParamsItem>();
}