using System.Linq.Expressions;

namespace WPF.Services.Validation;

public class ValidationRuleBuilder<T>
{
    private readonly List<IValidationRule<T>> _rules = new List<IValidationRule<T>>();
    private string _currentPropertyName;

    public ValidationRuleBuilder<T> ForProperty(Expression<Func<T, object>> propertySelector)
    {
        _currentPropertyName = GetPropertyName(propertySelector);
        return this;
    }

    public ValidationRuleBuilder<T> Required(string errorMessage = "Field is required")
    {
        _rules.Add(new RequiredRule<T>(_currentPropertyName, errorMessage));
        return this;
    }

    public ValidationRuleBuilder<T> Length(int min, int max, string errorMessage = "Invalid length")
    {
        _rules.Add(new LengthRule<T>(_currentPropertyName, min, max, errorMessage));
        return this;
    }

    public ValidationRuleBuilder<T> Custom(Func<T, bool> validationFunc, 
                                           string ruleName, 
                                           string errorMessage,
                                           ValidationSeverity severity = ValidationSeverity.Error)
    {
        _rules.Add(new CustomValidationRule<T>(_currentPropertyName, ruleName, errorMessage, severity, validationFunc));
        return this;
    }

    public ValidationRuleBuilder<T> Regex(string pattern, string errorMessage = "Invalid format")
    {
        _rules.Add(new RegexRule<T>(_currentPropertyName, pattern, errorMessage));
        return this;
    }

    public IEnumerable<IValidationRule<T>> Build() => _rules;

    private string GetPropertyName(Expression<Func<T, object>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
            return memberExpression.Member.Name;

        if (expression.Body is UnaryExpression unaryExpression && 
            unaryExpression.Operand is MemberExpression unaryMemberExpression)
            return unaryMemberExpression.Member.Name;

        throw new ArgumentException("Invalid property expression");
    }
}