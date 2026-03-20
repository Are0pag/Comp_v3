using System.ComponentModel;

namespace Comp.ModelData;

public enum CounterpartyType : byte
{
    /// <summary>
    /// Поставщик - Сторона, которая продает товары или услуги
    /// </summary>
    [Description("Поставщик")]
    Supplier,
    
    /// <summary>
    /// Заказчик - Сторона, которая покупает товары или услуги
    /// </summary>
    [Description("Заказчик")]
    Client
}