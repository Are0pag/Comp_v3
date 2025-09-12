using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WPF.Services.Validation;

public class ValidationWarning : ValidationError { }

public class UserValidator : Validator<UserRegistrationDto>
{
    public UserValidator() {
        var rules = CreateRules()
                   .ForProperty(u => u.Email)
                   .Required("Email is required")
                    // Правильный вызов - 2 параметра
                   .Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", "Invalid email format")
                    // Правильный вызов - 3 параметра
                   .Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
                          System.Text.RegularExpressions.RegexOptions.IgnoreCase, 
                          "Email must be in valid format")
            
                   .ForProperty(u => u.Username)
                   .Required("Username is required")
                   .Length(3, 20, "Username must be between 3-20 characters")
                   .Custom(u => !u.Username.Contains(" "), "NoSpaces", "Username cannot contain spaces")
            
                   .Build();

        foreach (var rule in rules) {
            AddRule(rule);
        }
    }
}

// Дополнительный класс для примера с телефоном
public class UserRegistrationDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; } // Добавим свойство для телефона
}

// Пример с телефоном
public class UserValidatorWithPhone : Validator<UserRegistrationDto>
{
    public UserValidatorWithPhone()
    {
        var rules = CreateRules()
                   .ForProperty(u => u.Phone)
                   .Regex(@"^\+?[1-9]\d{1,14}$", "Invalid phone number format")
            
                   .Build();

        foreach (var rule in rules)
        {
            AddRule(rule);
        }
    }
}

/*public class UserRegistrationDto
{
    public string Email { get; set; }
    public string Phone { get; set; }
}*/