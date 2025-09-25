using WPF.Services.Validation;

namespace Comp_v4.TableWindows.ConditionalDesignation.Overrided;

public class CdValidator : ValidatorBase<Comp.ModelData.TechnicalItems.ConditionalDesignation>
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