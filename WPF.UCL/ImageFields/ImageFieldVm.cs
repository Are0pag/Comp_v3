using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Comp_v4.CompCard.Vm;

public abstract class ImageFieldVmBase : ObservableObject
{
    protected BitmapImage? _image;
    protected string _butSelectText = "Выбрать";
    protected string _butClearText = "Удалить";
    protected string _butOpenText = "Открыть";

    public Action<object?> OpenAction { get; set; }
    public Action<object?>  ClearAction { get; set; }
    public Action<object?>  SelectAction { get; set; }

    public string? ImagePath { get; set; }
    
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
    
    
    public virtual void Select() {
        SelectAction?.Invoke(null);
    }
    
    
    public virtual void Open() {
        OpenAction?.Invoke(null);
    }

    
    public virtual void Clear() {
        ClearAction?.Invoke(null);
    }
}
    

public partial class ImageFieldVm : ImageFieldVmBase
{
    [RelayCommand]
    public override void Clear() {
        base.Clear();
    }

    [RelayCommand]
    public override void Open() {
        base.Open();
    }

    [RelayCommand]
    public override void Select() {
        base.Select();
    }
}