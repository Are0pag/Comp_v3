using Comp.ModelData;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.PaymentOrders;

public class PoValidator : ValidatorBase<PaymentOrder>
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .ForProperty(mu => mu.Order).Required()
                   .ForProperty(po => po.Date).Required()
                   .ForProperty(po => po.Number).Required()
                   .ForProperty(p => p.PaymentAmount).Custom(order => order.PaymentAmount > 0, ruleName: "Has Payment Amount")
                   .Build();
        
        foreach (var rule in rules) 
            AddRule(rule);
    }
}