using System.ComponentModel;

namespace Comp.ModelData;

/// <summary>
/// Статусы заказа поставщику
/// </summary>
public enum OrderStatus : byte
{
    [Description("Создан")]
    Created,
    [Description("Заказан")]
    Ordered,
    [Description("Получен")]
    Received,
    [Description("Архивный")]
    Archived
}