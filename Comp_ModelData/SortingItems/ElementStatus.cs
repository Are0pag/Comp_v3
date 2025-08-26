using System.Windows.Media;
using Utils.WPF;

namespace CL_Comp_ModelData.SortingItems;

public class ElementStatus : NotifyPropertyChanged
{
    protected string _name;
    protected string _colorOfTextHex;

    public ElementStatus(string name, string colorOfTextHex) {
        _name = name;
        _colorOfTextHex = colorOfTextHex;
    }
    
    public int IdDataBase { get; set; }

    public string Name {
        get => _name;
        set {
            if (value == _name) return;
            _name = value;
            OnPropertyChanged();
        }
    }

    public string ColorOfTextHex {
        get => _colorOfTextHex;
        set {
            if (value == _colorOfTextHex) return;
            _colorOfTextHex = value;
            OnPropertyChanged();
        }
    }
    
    // Свойство для удобной работы в WPF
    public SolidColorBrush ColorBrush => new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorOfTextHex));
}