using System.ComponentModel;

namespace Comp.ModelData;

/// <summary>
/// Статус получения позиции заказа
/// </summary>
public enum ReceiveStatus : byte 
{
    /// <summary>
    /// Не получено
    /// </summary>
    [Description("Не получено")]
    NotReceived,   
    
    /// <summary>
    /// Частично получено
    /// </summary>
    [Description("Частично получено")]
    PartiallyReceived, 
    
    /// <summary>
    /// Полностью получено
    /// </summary>
    [Description("Полностью получено")]
    FullyReceived,
    
    /// <summary>
    /// Избыточно получено
    /// </summary>
    [Description("Избыточно получено")]
    OverReceived,
}
