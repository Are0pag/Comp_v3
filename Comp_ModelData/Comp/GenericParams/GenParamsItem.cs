using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp.GenericParams;

[Table(nameof(GenParamsItem) + "s")]
public class GenParamsItem : NotifyPropertyChanged
{
    protected string _genKey;
    protected string _genValue;
    
    public GenParamsItem() { } // Конструктор по умолчанию для EF
    public GenParamsItem(string genKey, string genValue) {
        _genKey = genKey;
        _genValue = genValue;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int GenParamsSetId { get; set; }
    
    // Навигационное свойство
    [ForeignKey(nameof(GenParamsSetId))]
    public virtual GenParamsSet GenParamsSet { get; set; }
    
    public string GenKey {
        get => _genKey;
        set {
            if (_genKey == value) return;
            _genKey = value;
            OnPropertyChanged();
        }
    }

    public string GenValue {
        get => _genValue;
        set {
            if (_genValue == value) return;
            _genValue = value;
            OnPropertyChanged();
        }
    }
}