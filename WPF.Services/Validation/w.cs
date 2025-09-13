using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WPF.Services.Validation;

public class ValidationWarning : ValidationError { }

// Статический helper класс для удобства
public static class Pattern
{
    public static RegexPatternBuilder Create() => RegexPatternBuilder.Create();
    
    public static string Email() => Create().Email().Build();
    public static string Phone() => Create().Phone().Build();
    
    public static string SimpleEmail() => 
        Create().StartsWith().Alphanumeric(1).Text("@").Alphanumeric(1).Text(".").Letters(2).EndsWith().Build();
}
public class UserValidator : ValidatorBase<UserRegistrationDto>
{
    public UserValidator() {
        var emailPattern = Pattern.Create()
                                  .StartsWith()    //  ^
                                  .Alphanumeric(1) // [a-zA-Z0-9]{1}  (минимум 1 буква/цифра)
                                  .Text("@")       // @
                                  .Alphanumeric(1) // [a-zA-Z0-9]{1} (минимум 1 буква/цифра)
                                  .Text(".")       // \.
                                  .Letters(2, 4)   // [a-zA-Z]{2,4} (от 2 до 4 букв)
                                  .EndsWith()      // $
                                  .Build();        // ^[a-zA-Z0-9]{1}@[a-zA-Z0-9]{1}\.[a-zA-Z]{2,4}$

        var phonePattern = Pattern.Create()
                                  .StartsWith()
                                  .Text("+").Optional()
                                  .Digits(1, 3)
                                  .Digits(10)
                                  .EndsWith()
                                  .Build(); // ^\+?\d{1,3}\d{10}$

        
        // Сложный пример с группами
        var complexPattern = Pattern.Create()
                                    .StartsWith()
                                    .Group(b => b
                                               .Text("http")
                                               .Text("s").Optional()
                                               .Text("://")
                                     )
                                    .Alphanumeric(1)
                                    .Text(".")
                                    .Letters(2, 4)
                                    .EndsWith()
                                    .Build();
        
        var rules = CreateRules()
                   .ForProperty(u => u.Email)
                   .Required("Email is required")
                    // Правильный вызов - 2 параметра
                   .Regex(/*@"^[^@\s]+@[^@\s]+\.[^@\s]+$"*/ emailPattern, "Invalid email format")
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

        if (ValidateAsync(new UserRegistrationDto()).Result is { IsValid: true } result) {
            
        }
        
        switch (ValidateAsync(new UserRegistrationDto()).Result) {
            case {IsValid: true}:
                
                break;
        }
    }

    protected override void AddRules() {
        throw new NotImplementedException();
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
public class UserValidatorWithPhone : ValidatorBase<UserRegistrationDto>
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

    protected override void AddRules() {
        throw new NotImplementedException();
    }
}

/*public class UserRegistrationDto
{
    public string Email { get; set; }
    public string Phone { get; set; }
}*/