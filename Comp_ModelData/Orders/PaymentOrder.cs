using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData.TechnicalItems;

namespace Comp.ModelData;

/// <summary>
/// Платёжное поручение
/// </summary>
[Table("PaymentOrders")]
public class PaymentOrder : ObservableObject, IDbEntity, IPopulatable<PaymentOrder>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int OrderId { get; set; }

    protected SupplierOrder _order;
    protected DateTime _date;
    protected string _number;
    protected decimal _paymentAmount;
    protected string? _paymentPurpose;

    /// <summary>
    /// Заказ, на который нацелено это платёжное поручение
    /// </summary>
    [ForeignKey(nameof(OrderId))]
    public SupplierOrder Order {
        get => _order;
        set {
            if (_order == value) return;
            _order = value;
            OnPropertyChanged();
        }
    }
    
    public DateTime Date {
        get => _date;
        set {
            if (_date == value) return;
            _date = value;
            OnPropertyChanged();
        }
    }
    
    /// <summary>
    /// Номер платёжного поручения
    /// </summary>
    public string Number {
        get => _number;
        set {
            if (_number == value) return;
            _number = value;
            OnPropertyChanged();
        }
    }
    
    public decimal PaymentAmount {
        get => _paymentAmount;
        set {
            if (_paymentAmount == value) return;
            _paymentAmount = value;
            OnPropertyChanged();
        }
    }
    
    /// <summary>
    /// Назначение платежа
    /// </summary>
    public string? PaymentPurpose {
        get => _paymentPurpose;
        set {
            _paymentPurpose = value;
            OnPropertyChanged();
        }
    }
    
    
    public PaymentOrder PopulateFrom(PaymentOrder targetValues) {
        Id = targetValues.Id;
        
        OrderId = targetValues.OrderId;
        Order = targetValues.Order;
        
        Date = targetValues.Date;
        Number = targetValues.Number;
        PaymentAmount = targetValues.PaymentAmount;
        PaymentPurpose = targetValues.PaymentPurpose;
        
        return this;
    }
}