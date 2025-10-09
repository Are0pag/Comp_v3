using Comp.ModelData.TechnicalItems;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.GenericParametersSets;

public class gpsValidator : ValidatorBase<GenericParametersSet>
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .ForProperty(mu => mu.Name)
                   .Required()
                   .Build();
        foreach (var rule in rules) 
            AddRule(rule);
    }
}