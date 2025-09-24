using Comp.ModelData.TechnicalItems;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.Manufacturers.Overrided;

public class Validator : ValidatorBase<Manufacturer>
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .ForProperty(m => m.Designation)
                   .Required()
                   .Build();
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}