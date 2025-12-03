namespace Comp.ModelData;

/// <summary>
/// Статус получения позиции заказа
/// </summary>
public enum OrderPositionReceiveStatus {
    /// <summary>
    /// Не получено
    /// </summary>
    NotReceived,   
    
    /// <summary>
    /// Частично получено
    /// </summary>
    PartiallyReceived, 
    
    /// <summary>
    /// Полностью получено
    /// </summary>
    FullyReceived,
}