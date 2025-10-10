using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.CompCard.Vm;

public partial class ImageFieldVm : ObservableObject
{
    protected BitmapImage? _image;
    protected string _butSelectText = "Выбрать";
    protected string _butClearText = "Удалить";
    protected string _butOpenText = "Открыть";

    public Action<object?> OpenAction { get; set; }
    public Action<object?>  ClearAction { get; set; }
    public Action<object?>  SelectAction { get; set; }

    public BitmapImage? Image {
        get => _image;
        set {
            if (_image == value) return;
            _image = value;
            OnPropertyChanged();
        }
    }
    
    public string ButSelectText {
        get => _butSelectText;
        set {
            _butSelectText = value;
            OnPropertyChanged();
        }
    }

    public string ButClearText {
        get => _butClearText;
        set {
            _butClearText = value;
            OnPropertyChanged();
        }
    }

    public string ButOpenText {
        get => _butOpenText;
        set {
            _butOpenText = value;
            OnPropertyChanged();
        }
    }
    
    [RelayCommand]
    public void Select() {
        SelectAction?.Invoke(null);
    }
    
    [RelayCommand]
    public void Open() {
        OpenAction?.Invoke(null);
    }

    [RelayCommand]
    public void Clear() {
        ClearAction?.Invoke(null);
    }
}