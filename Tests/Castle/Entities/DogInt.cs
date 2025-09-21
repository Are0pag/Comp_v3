using CommunityToolkit.Mvvm.ComponentModel;

namespace Tests;

public class ViewModel {}

public class DogInt : ObservableObject
{
    public DogInt(int value, Cat cat) {
        Console.WriteLine($"Value: {value}");
    }
}


public interface ICat {}
public class Cat : ICat { }

public interface ICatMom : IDisposable {}
public class CatMom : ICatMom
{
    protected readonly ICat _cat;

    public CatMom(ICat cat) {
        _cat = cat;
    }

    public void Dispose() {
        
    }
}

public class WildBeer { }