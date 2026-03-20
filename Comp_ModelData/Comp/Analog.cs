using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;

namespace Comp.ModelData;

[Table(nameof(Analog) + "s")]
public class Analog : ObservableObject, IDbEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    protected Component _sourceComponent;
    protected Component _analog;
    protected bool _isAllCount;
    
    public int SourceComponentId { get; set; }
    public int RelatedComponentId { get; set; }

    [ForeignKey(nameof(SourceComponentId))]
    public Component SourceComponent {
        get => _sourceComponent;
        set {
            if (_sourceComponent == value) return;
            _sourceComponent = value;
            OnPropertyChanged();
        }
    }

    [ForeignKey(nameof(RelatedComponentId))]
    public Component RelatedComponent {
        get => _analog;
        set {
            if (_analog == value) return;
            _analog = value;
            OnPropertyChanged();
        }
    }

    public bool IsAllCount {
        get => _isAllCount;
        set {
            if (_isAllCount == value) return;
            _isAllCount = value;
            OnPropertyChanged();
        }
    }
}