using System.Text.RegularExpressions;

namespace WPF.Services.Validation;

public class RegexPatternBuilder
{
    private string _pattern = string.Empty;

    static public RegexPatternBuilder Create() {
        return new RegexPatternBuilder();
    }

    public RegexPatternBuilder StartsWith() {
        _pattern += "^";
        return this;
    }

    public RegexPatternBuilder EndsWith() {
        _pattern += "$";
        return this;
    }

    public RegexPatternBuilder Text(string text) {
        _pattern += Regex.Escape(text);
        return this;
    }

    public RegexPatternBuilder AnyCharacter() {
        _pattern += ".";
        return this;
    }

    public RegexPatternBuilder Digits(int min = 1, int? max = null) {
        _pattern += max.HasValue ? $"\\d{{{min},{max}}}" : $"\\d{{{min}}}";
        return this;
    }

    public RegexPatternBuilder Letters(int min = 1, int? max = null) {
        _pattern += max.HasValue ? $"[a-zA-Z]{{{min},{max}}}" : $"[a-zA-Z]{{{min}}}";
        return this;
    }

    public RegexPatternBuilder Alphanumeric(int min = 1, int? max = null) {
        _pattern += max.HasValue 
            ? $"[a-zA-Zа-яА-ЯёЁ0-9]{{{min},{max}}}" 
            : $"[a-zA-Zа-яА-ЯёЁ0-9]{{{min}}}";
        return this;
    }

    public RegexPatternBuilder OneOf(params string[] characters) {
        var escaped = characters.Select(Regex.Escape);
        _pattern += $"[{string.Join("", escaped)}]";
        return this;
    }

    public RegexPatternBuilder NotOneOf(params string[] characters) {
        var escaped = characters.Select(Regex.Escape);
        _pattern += $"[^{string.Join("", escaped)}]";
        return this;
    }

    public RegexPatternBuilder Optional() {
        _pattern += "?";
        return this;
    }

    public RegexPatternBuilder ZeroOrMore() {
        _pattern += "*";
        return this;
    }

    public RegexPatternBuilder OneOrMore() {
        _pattern += "+";
        return this;
    }

    public RegexPatternBuilder Exactly(int count) {
        _pattern += $"{{{count}}}";
        return this;
    }

    public RegexPatternBuilder Between(int min, int max) {
        _pattern += $"{{{min},{max}}}";
        return this;
    }

    public RegexPatternBuilder AtLeast(int min) {
        _pattern += $"{{{min},}}";
        return this;
    }

    public RegexPatternBuilder Email() {
        _pattern += @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";
        return this;
    }

    public RegexPatternBuilder Phone() {
        _pattern += @"\+?[1-9]\d{1,14}";
        return this;
    }

    public RegexPatternBuilder Group(Action<RegexPatternBuilder> groupBuilder) {
        _pattern += "(";
        var innerBuilder = new RegexPatternBuilder();
        groupBuilder(innerBuilder);
        _pattern += innerBuilder.Build() + ")";

        return this;
    }

    public RegexPatternBuilder CustomPattern(string pattern) {
        _pattern += pattern;
        return this;
    }
    
    public string Build() {
        return _pattern;
    }
}