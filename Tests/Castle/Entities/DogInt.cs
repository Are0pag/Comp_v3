using CommunityToolkit.Mvvm.ComponentModel;

namespace Tests;

public class ViewModel {}

public class DogInt : ObservableObject, IDisposable
{
    protected readonly ICat _cat;
    public DogInt(int value, ICat cat, string name) {
        _cat = cat;
        Console.WriteLine($"Value: {value}");
        Console.WriteLine($"Name: {name}");
    }

    public void Dispose() {
        _cat.Dispose();
    }
}


public interface ICat : IDisposable {}
public class Cat : ICat
{
    public void Dispose() {
        
    }
}

public interface ICatMom : IDisposable {}
public class CatMom : ICatMom
{
    protected readonly ICat _cat;

    public CatMom(ICat cat) {
        _cat = cat;
    }

    public void Dispose() {
        GC.SuppressFinalize(this);
    }
}

public class WildBeer : IDisposable
{
    public WildBeer() {
        Console.WriteLine("Wild Beer have non lazy registration!");
    }
    public void Dispose() {
        
    }
}

