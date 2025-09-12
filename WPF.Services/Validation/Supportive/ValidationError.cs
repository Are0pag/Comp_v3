namespace WPF.Services.Validation;

public class ValidationError
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    public ValidationSeverity Severity { get; set; }
    public string RuleName { get; set; }
}