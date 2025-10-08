using WPF.Services.ValidationString;

namespace Comp_v4.CompCard.Entities.Validation;

public class ValidatorName : StringValidatorBase
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .Required()
                   .Build();
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}

public class ValidatorNomNumber : StringValidatorBase
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .Required()
                   .Build();
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}

public class ValidatorCatalogNumber : StringValidatorBase
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .Required()
                   .Build();
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}

public class ValidatorLabelingOptions : StringValidatorBase
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .Required()
                   .Build();
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}

public class ValidatorCodeOfElement : StringValidatorBase
{
    protected override void SetRules() {
        var rules = CreateRules()
                   .Required()
                   .Build();
        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}

