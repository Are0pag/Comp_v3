using CommunityToolkit.Mvvm.ComponentModel;

namespace Tests;

public class FloatingVm : ObservableObject
{
    public FloatingVm(int value) {
        Console.WriteLine($"Value: {value}");
    }
}