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