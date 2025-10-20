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

#region Requisites

    protected string _counterpartyTypeName;
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
        get => _counterpartyTypeName;
        set {
            _counterpartyTypeName = value;
            OnPropertyChanged();
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
}