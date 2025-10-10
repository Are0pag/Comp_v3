namespace Comp_v4.CompCard.Vm.Buttons;

public abstract class LinkFieldControlVm : SetLinkButtonVm
{
    private string _fieldTitle;
    private string _url = string.Empty;
    
    public string FieldTitle {
        get => _fieldTitle;
        set {
            _fieldTitle = value;
            OnPropertyChanged(nameof(FieldTitle));
        }
    }

    public string Url {
        get => _url;
        set {
            _url = value;
            OnPropertyChanged(nameof(Url));
        }
    }
}

public class UrlFieldControlVm : LinkFieldControlVm
{
    public UrlFieldControlVm() {
        FieldTitle = "Интернет ссылка: ";
    }
}

public class UrlAlternativeFieldControlVm : LinkFieldControlVm
{
    public UrlAlternativeFieldControlVm() {
        FieldTitle = "Альтернативная ссылка: ";
    }
}

public class FilePathFieldControlVm : LinkFieldControlVm
{
    public FilePathFieldControlVm() {
        FieldTitle = "Файл спецификации: ";
    }
}

