using Comp.ModelData.SortingItems;
using WPF.Services.Validation;

namespace Comp_v4.NomDict.Entities;

public class CategoryValidator : ValidatorBase<Category>
{
    protected override void SetRules() {
        var namePattern = RegexPatternBuilder.Create()
                                             .CustomPattern(@"^[a-zA-Zа-яА-ЯёЁ0-9][a-zA-Zа-яА-ЯёЁ0-9 .]*[a-zA-Zа-яА-ЯёЁ0-9]$")
                                             .Build();
        
        var rules = CreateRules()
                   .ForProperty(c => c.Name)
                   .Required("Название категории обязательно")
                   .Length(3, 50, "Название категории должно содержать от 3 до 50 символов")
                   .Regex(namePattern, "Название категории может содержать буквы, цифры, пробелы и точки")
                   .Custom(category => !category.Name.Contains(".."), "NoDuplicateDots", "Точки не могут идти подряд")
                   .Custom(category => !category.Name.Contains("  "), "NoDoublespaces", "Пробелы не могут идти подряд")
                   .Custom(category => !category.Name.EndsWith("."), "NoEdgeDots", "Название не может заканчиваться точкой")
                   .Custom(category => !category.Name.StartsWith(" ") && !category.Name.EndsWith(" "), 
                           "NoEdgeSpaces", "Название не может начинаться или заканчиваться пробелом")
                   .Build();

        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}


