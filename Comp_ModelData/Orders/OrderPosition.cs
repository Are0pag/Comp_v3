using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;

namespace Comp.ModelData;

/// <summary>
/// Спецификация заказа
/// </summary>
[Table("OrderPositions")]
public class OrderPosition : ObservableObject, IDbEntity, IPopulatable<OrderPosition>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

#region foreing

    public int PositionId { get; set; }
    
    protected Component _position;

    [ForeignKey(nameof(PositionId))]
    public Component Position {
        get => _position;
        set {
            if (_position == value) return;
            _position = value;
            OnPropertyChanged();
        }
    }

#endregion

#region quantities

    protected int _orderQuantity;
    protected int _receivedQuantity;
    protected OrderPositionReceiveStatus _receiveStatus;

    public int OrderQuantity {
        get => _orderQuantity;
        set {
            if (_orderQuantity == value) return;
            _orderQuantity = value;
            OnPropertyChanged();
        }
    }

    public int ReceivedQuantity {
        get => _receivedQuantity;
        set {
            if (_receivedQuantity == value) return;
            _receivedQuantity = value;
            OnPropertyChanged();
        }
    }
    
    public string ReceiveStatus {
        get => _receiveStatus.ToString();
        set {
            foreach (OrderPositionReceiveStatus rs in Enum.GetValues(typeof(OrderPositionReceiveStatus))) {
                if (rs.ToString() != value)
                    continue;
                _receiveStatus = rs;
                OnPropertyChanged();
                return;
            }
            throw new ArgumentException($"Unknown {nameof(OrderPositionReceiveStatus)} type: {value}");
        }
    }

#endregion

#region cost

    protected decimal _unitPrice;
    protected decimal _totalCost;

    public decimal UnitPrice {
        get => _unitPrice;
        set {
            if (_unitPrice == value) return;
            _unitPrice = value;
            OnPropertyChanged();
        }
    }

    public decimal TotalCost {
        get => _totalCost;
        set {
            if (_totalCost == value) return;
            _totalCost = value;
            OnPropertyChanged();
        }
    }

#endregion

#region populating

    public OrderPosition PopulateFrom(OrderPosition targetValues) {
        Id = targetValues.Id;
        
        PositionId = targetValues.PositionId;
        Position = targetValues.Position;
        
        OrderQuantity = targetValues.OrderQuantity;
        ReceivedQuantity = targetValues.ReceivedQuantity;
        ReceiveStatus = targetValues.ReceiveStatus;
        
        UnitPrice = targetValues.UnitPrice;
        TotalCost = targetValues.TotalCost;
        return this;
    }

#endregion
}