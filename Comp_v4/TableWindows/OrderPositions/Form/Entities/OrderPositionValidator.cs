using Comp.ModelData;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.OrderPositions.Form.Entities;

public class OrderPositionValidator : ValidatorBase<OrderPosition>
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .ForProperty(op => op.Position)
                   .Required()
                   .Build();
        
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}