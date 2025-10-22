namespace Comp.ModelData;

/// <summary>
/// Статусы заказа поставщику
/// </summary>
public enum OrderStatus
{
    /// Создан
    Created,
    /// Заказан
    Ordered,
    /// Получен
    Received,
    /// Архивный
    Archived
}