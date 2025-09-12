namespace WPF.Services.Validation;

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
    public List<ValidationWarning> Warnings { get; set; } = new List<ValidationWarning>();
}