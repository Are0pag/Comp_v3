using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.CompCard;

public partial class CompCardVm : ObservableObject
{
    private string _name = "Say me name";

    public string UserName {
        get => _name;
        set {
            _name = value;
            OnPropertyChanged();
        }
    }
    
    [RelayCommand]
    private void GenerateUserName() {
        UserName = $"User_{System.DateTime.Now:HHmmss}";
    }
}