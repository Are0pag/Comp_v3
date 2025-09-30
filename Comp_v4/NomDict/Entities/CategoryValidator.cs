using Comp.ModelData.SortingItems;
using WPF.Services.Validation;

namespace Comp_v4.NomDict.Entities;

public class CategoryValidator : ValidatorBase<Category>
{
    protected override void SetRules() {
        var namePattern = Pattern.Create()
                                 .StartsWith()
                                 .Alphanumeric(3, 50)  // От 3 до 50 символов (буквы и цифры)
                                 .EndsWith()
                                 .Build();
        
        var rules = CreateRules()
                   .ForProperty(c => c.Name)
                   .Required("Название категории обязательно")
                   .Length(3, 50, "Название категории должно содержать от 3 до 50 символов")
                   .Regex(namePattern, "Название категории может содержать только буквы и цифры")
                   .Build();

        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}