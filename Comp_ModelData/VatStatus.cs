namespace Comp.ModelData;

/// <summary>
/// Статусы НДС для заказа
/// </summary>
public enum VatStatus
{
    /// Без НДС
    WithoutVat,
    /// НДС включён
    VatIncluded,
    /// НДС сверху
    VatOnTop
}