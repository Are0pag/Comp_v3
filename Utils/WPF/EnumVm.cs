using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Utils.WPF;

public class EnumVm<T> : ObservableObject
    where T : Enum
{
    public ObservableCollection<T> EnumValues { get; } 
        = new(Enum.GetValues(typeof(T)).Cast<T>());

    protected T _selectedValue;
    public virtual T SelectedValue
    {
        get => _selectedValue;
        set => SetProperty(ref _selectedValue, value);
    }
}

public class EnumVmSourceChanging<TEnum, TSource> : EnumVm<TEnum>
    where TEnum : Enum
{
    protected readonly TSource _source;

    public EnumVmSourceChanging(TSource source) {
        _source = source;
    }
}