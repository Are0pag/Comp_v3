using Comp.ModelData.TechnicalItems;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.TypeSizes;

public class tsValidator : ValidatorBase<TypeSize>
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .ForProperty(mu => mu.Designation)
                   .Required()
                   .Build();
        foreach (var rule in rules) 
            AddRule(rule);
    }
}