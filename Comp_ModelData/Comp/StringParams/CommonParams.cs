using Utils.WPF;

namespace CL_Comp_ModelData.Comp.StringParams;

public class CommonParams : NotifyPropertyChanged
{
    protected string _catalogNumber = "-";
    protected string _labelingOptions = "-";
    protected string _codeElement = "-";
    protected string _qrCodeData = "-";
    public string CatalogNumber {
        get => _catalogNumber;
        set {
            if (value == _catalogNumber)
                return;
            _catalogNumber = value;
            OnPropertyChanged();
        }
    }
    public string LabelingOptions {
        get => _labelingOptions;
        set {
            if (value == _labelingOptions)
                return;
            _labelingOptions = value;
            OnPropertyChanged();
        }
    }
    public string CodeElement {
        get => _codeElement;
        set {
            if (value == _codeElement)
                return;
            _codeElement = value;
            OnPropertyChanged();
        }
    }
    public string QrCodeData {
        get => _qrCodeData;
        set {
            if (value == _qrCodeData)
                return;
            _qrCodeData = value;
            OnPropertyChanged();
        }
    }
}
