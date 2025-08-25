using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Utils.WPF;

public abstract class NotifyPropertyChanged : INotifyPropertyChanged 
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        //Console.WriteLine($"PropertyChanged: {propertyName}");
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}