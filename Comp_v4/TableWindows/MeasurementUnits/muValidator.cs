using Comp.ModelData.TechnicalItems;
using WPF.Services.Validation;

namespace Comp_v4.TableWindows.MeasurementUnits;

public class muValidator : ValidatorBase<MeasurementUnit>
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