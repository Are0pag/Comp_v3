using CommunityToolkit.Mvvm.ComponentModel;

namespace Tests;

public class DogInt : ObservableObject
{
    public DogInt(int value, Cat cat) {
        Console.WriteLine($"Value: {value}");
    }
}

public class Cat
{
    
}

public class CatMom
{
    protected readonly Cat _cat;

    public CatMom(Cat cat) {
        _cat = cat;
    }
}