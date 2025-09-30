using Comp.ModelData.SortingItems;
using WPF.Services.Validation;

namespace Comp_v4.NomDict.Entities;

public class CategoryValidator : ValidatorBase<Category>
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .ForProperty(c => c.Name)
                   .Length(3, 5)
                   .Build();

        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}