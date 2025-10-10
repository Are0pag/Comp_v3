namespace Comp_v4.CompCard.Vm.Buttons;

public partial class LinkFieldControlVm : SetLinkButtonVm
{
    private string _fieldTitle;
    private string _url;

    public LinkFieldControlVm() {
        _fieldTitle = "Link";
        _url = string.Empty;
    }
    
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