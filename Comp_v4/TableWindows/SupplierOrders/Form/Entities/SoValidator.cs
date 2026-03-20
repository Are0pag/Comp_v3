using Comp.ModelData;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Entities;

public class SoValidator : ValidatorBase<SupplierOrder>
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .ForProperty(so => so.PurchaseOrderNumber)
                   .Required()
                   .ForProperty(so => so.Counterparty)
                   .Required()
                   .Build();
        
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}