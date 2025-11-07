using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData.TechnicalItems;

namespace Comp.ModelData;

/// <summary>
/// Заказ поставщику - документ, описывающий детали закупки товаров или услуг
/// </summary>
[Table("SupplierOrders")]
public class SupplierOrder : ObservableObject, IDbEntity
{
    public SupplierOrder() {
        OrderDate = DateTime.Now;
        DeliveryDate = DateTime.Now;
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Bindings +    
#region TextData

    protected string _purchaseOrderNumber;
    protected string? _invoiceNumber;
    protected string? _note;

    /// <summary>
    /// Номер заказа
    /// </summary>
    public string PurchaseOrderNumber {
        get => _purchaseOrderNumber;
        set {
            _purchaseOrderNumber = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Номер счёта
    /// </summary>
    public string? InvoiceNumber {
        get => _invoiceNumber;
        set {
            _invoiceNumber = value;
            OnPropertyChanged();
        }
    }

    public string? Note {
        get => _note;
        set {
            _note = value;
            OnPropertyChanged();
        }
    }

#endregion
    
    // Bindings +    
#region EnumsData

    protected OrderStatus _orderStatus;
    protected VatStatus _vatStatus;

    public string OrderStatus {
        get => _orderStatus.ToString();
        set {
            foreach (OrderStatus os in Enum.GetValues(typeof(OrderStatus))) {
                if (os.ToString() != value)
                    continue;
                _orderStatus = os;
                OnPropertyChanged();
                return;
            }
            throw new ArgumentException($"Unknown {nameof(OrderStatus)} type: {value}");
        }
    }

    /// <summary>
    /// Статус НДС
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public string VatStatus {
        get => _vatStatus.ToString();
        set {
            foreach (VatStatus vs in Enum.GetValues(typeof(VatStatus))) {
                if (vs.ToString() != value)
                    continue;
                _vatStatus = vs;
                OnPropertyChanged();
                return;
            }
            throw new ArgumentException($"Unknown {nameof(VatStatus)} type: {value}");
        }
    }

#endregion

#region PathsData

    protected string? _contractFilePath;
    protected string? _invoiceFilePath;

    /// <summary>
    /// Путь к файлу договора (локальная или интернет-ссылка)
    /// </summary>
    public string? ContractFilePath {
        get => _contractFilePath;
        set {
            _contractFilePath = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Путь к файлу счёта (локальная или интернет-ссылка)
    /// </summary>
    public string? InvoiceFilePath {
        get => _invoiceFilePath;
        set {
            _invoiceFilePath = value;
            OnPropertyChanged();
        }
    }

#endregion
    
    // Bindings +    
#region DateTimeData

    protected DateTime _orderDate;
    protected DateTime _deliveryDate;

    /// <summary>
    /// Дата заказа
    /// </summary>
    public DateTime OrderDate {
        get => _orderDate;
        set {
            _orderDate = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    /// Дата поставки
    /// </summary>
    public DateTime DeliveryDate {
        get => _deliveryDate;
        set {
            _deliveryDate = value;
            OnPropertyChanged();
        }
    }

#endregion

#region ForeingData

    public int CounterpartyId { get; set; }
    
    protected Counterparty _counterparty;

    /// <summary>
    /// Поставщик (внешний ключ)
    /// </summary>
    [ForeignKey(nameof(CounterpartyId))]
    public Counterparty Counterparty {
        get => _counterparty;
        set {
            if (_counterparty == value) return;
            _counterparty = value;
            OnPropertyChanged();
        }
    }

#endregion

    
#region Copy
    
    public SupplierOrder CopyTo(SupplierOrder target) {
        target.Id = this.Id;
        
        // Текстовые данные
        target.PurchaseOrderNumber = this.PurchaseOrderNumber;
        target.InvoiceNumber = this.InvoiceNumber;
        target.Note = this.Note;

        // Статусы
        target.OrderStatus = this.OrderStatus;
        target.VatStatus = this.VatStatus;

        // Пути к файлам
        target.ContractFilePath = this.ContractFilePath;
        target.InvoiceFilePath = this.InvoiceFilePath;

        // Даты
        target.OrderDate = this.OrderDate;
        target.DeliveryDate = this.DeliveryDate;
        
        target.Counterparty = this.Counterparty;
        target.CounterpartyId = this.CounterpartyId;
        
        return target;
    }

#endregion
}