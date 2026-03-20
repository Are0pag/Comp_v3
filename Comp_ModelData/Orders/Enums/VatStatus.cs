using System.ComponentModel;

namespace Comp.ModelData;

/// <summary>
/// Статусы НДС для заказа
/// </summary>
public enum VatStatus : byte
{
    [Description("Без НДС")]
    WithoutVat,
    [Description("НДС включён")]
    VatIncluded,
    [Description("НДС сверху")]
    VatOnTop
}