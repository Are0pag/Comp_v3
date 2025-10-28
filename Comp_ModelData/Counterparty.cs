using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData.TechnicalItems;

namespace Comp.ModelData;

/// <summary>
/// Контрагент - юридическое или физическое лицо, которое участвует в договорных отношениях с компанией на поставку товаров, работ или услуг
/// </summary>
[Table("Counterparties")]
public class Counterparty : ObservableObject, IDbEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    // Bindings +
#region Requisites

    protected CounterpartyType _counterpartyType;
    protected string _shortName; // unique !
    protected string? _fullName; // unique ! (but not requared)
    protected string? _cityName;
    protected string? _address;
    
    /// <summary>
    /// TIN (Tax Identification Number) - ИНН - Налоговый идентификационный номер
    /// </summary>
    protected string? _tin;

    /// <summary>
    /// КПП - Код причины постановки на учет
    /// </summary>
    protected string? _reasonCode;
    
    public string CounterpartyTypeName {
        get => _counterpartyType.ToString();
        set {
            foreach (CounterpartyType c in Enum.GetValues(typeof(CounterpartyType))) {
                if (c.ToString() != value)
                    continue;
                _counterpartyType = c;
                OnPropertyChanged();
                return;
            }
            throw new ArgumentException($"Unknown counterparty type: {value}");
        }
    }

    public string ShortName {
        get => _shortName;
        set {
            _shortName = value;
            OnPropertyChanged();
        }
    }

    public string? FullName {
        get => _fullName;
        set {
            _fullName = value;
            OnPropertyChanged();
        }
    }

    public string? CityName {
        get => _cityName;
        set {
            _cityName = value;
            OnPropertyChanged();
        }
    }

    public string? Address {
        get => _address;
        set {
            _address = value;
            OnPropertyChanged();
        }
    }

    public string? Tin {
        get => _tin;
        set {
            _tin = value;
            OnPropertyChanged();
        }
    }

    public string? ReasonCode {
        get => _reasonCode;
        set {
            _reasonCode = value;
            OnPropertyChanged();
        }
    }

#endregion

    // Bindings +
#region Account

    protected string? _bankName;
    /// <summary>
    /// Банковский счет для финансовых операций (рассчётный счёт)
    /// </summary>
    protected string? _settlementAccount;
    protected string? _minimumOrderAmount;
    /// <summary>
    /// Плательщик НДС
    /// </summary>
    protected bool? _isVatTaxpayer;

    public string? BankName {
        get => _bankName;
        set {
            _bankName = value;
            OnPropertyChanged();
        }
    }

    public string? SettlementAccount {
        get => _settlementAccount;
        set {
            _settlementAccount = value;
            OnPropertyChanged();
        }
    }

    public string? MinimumOrderAmount {
        get => _minimumOrderAmount;
        set {
            _minimumOrderAmount = value;
            OnPropertyChanged();
        }
    }

    public bool? IsVatTaxpayer {
        get => _isVatTaxpayer;
        set {
            _isVatTaxpayer = value;
            OnPropertyChanged();
        }
    }

#endregion
    
    // Bindings +
#region Contacts

    protected string? _phoneNumber;
    protected string? _email;
    protected string? _webSite;
    protected string? _webSiteLogin;
    protected string? _webSitePassword;
    protected string? _comment;
    
    public string? PhoneNumber {
        get => _phoneNumber;
        set {
            _phoneNumber = value;
            OnPropertyChanged();
        }
    }

    public string? Email {
        get => _email;
        set {
            _email = value;
            OnPropertyChanged();
        }
    }

    public string? Website {
        get => _webSite;
        set {
            _webSite = value;
            OnPropertyChanged();
        }
    }

    public string? WebsiteLogin {
        get => _webSiteLogin;
        set {
            _webSiteLogin = value;
            OnPropertyChanged();
        }
    }

    public string? WebsitePassword {
        get => _webSitePassword;
        set {
            _webSitePassword = value;
            OnPropertyChanged();
        }
    }

    public string? Comment {
        get => _comment;
        set {
            _comment = value;
            OnPropertyChanged();
        }
    }

#endregion

#region Copy

public Counterparty PopulateFrom(Counterparty targetValues) {
    if (targetValues == null)
        throw new ArgumentNullException(nameof(targetValues));

    // Копирование значений из targetValues
    CounterpartyTypeName = targetValues.CounterpartyTypeName;
    ShortName = targetValues.ShortName;
    FullName = targetValues.FullName;
    CityName = targetValues.CityName;
    Address = targetValues.Address;
    Tin = targetValues.Tin;
    ReasonCode = targetValues.ReasonCode;

    BankName = targetValues.BankName;
    SettlementAccount = targetValues.SettlementAccount;
    MinimumOrderAmount = targetValues.MinimumOrderAmount;
    IsVatTaxpayer = targetValues.IsVatTaxpayer;

    PhoneNumber = targetValues.PhoneNumber;
    Email = targetValues.Email;
    Website = targetValues.Website;
    WebsiteLogin = targetValues.WebsiteLogin;
    WebsitePassword = targetValues.WebsitePassword;
    Comment = targetValues.Comment;

    return this; // Вернуть текущий экземпляр для возможности цепочного вызова
}

#endregion

}