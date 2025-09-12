using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WPF.Services.Validation;

public class ValidationWarning : ValidationError { }

public class UserValidator : Validator<UserRegistrationDto>
{
    public UserValidator()
    {
        var rules = CreateRules()
                   .ForProperty(u => u.Email)
                   .Required("Email is required")
                   .Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", "Invalid email format")
                   .Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
                          System.Text.RegularExpressions.RegexOptions.IgnoreCase, 
                          "Email must be in valid format")
            
                   .ForProperty(u => u.Phone)
                   .Regex(@"^\+?[1-9]\d{1,14}$", "Invalid phone number format")
            
                   .Build();

        foreach (var rule in rules)
        {
            AddRule(rule);
        }
    }
}

public class UserRegistrationDto
{
    public string Email { get; set; }
    public string Phone { get; set; }
}