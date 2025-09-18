using Comp.ModelData.TechnicalItems;
using WPF.Services.Validation;

namespace Comp_v4.Entities;

public class Validator : ValidatorBase<ConditionalDesignation>
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .ForProperty(cd => cd.Designation)
                   .Required().Build();
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}