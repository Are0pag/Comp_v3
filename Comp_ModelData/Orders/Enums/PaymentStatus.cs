using System.ComponentModel;

namespace Comp.ModelData;

/// <summary>
/// Статус оплаты
/// </summary>
public enum PaymentStatus : byte
{
    /// <summary>
    /// Не оплачено
    /// </summary>
    [Description("Не оплачено")]
    NotPayed,
    
    /// <summary>
    /// Частично оплачено
    /// </summary>
    [Description("Частично оплачено")]
    PartiallyPayed,
    
    /// <summary>
    /// Полностью оплачено
    /// </summary>
    [Description("Полностью оплачено")]
    FullyPayed,
    
    /// <summary>
    /// Переплачено
    /// </summary>
    [Description("Переплачено")]
    Overpaid
}