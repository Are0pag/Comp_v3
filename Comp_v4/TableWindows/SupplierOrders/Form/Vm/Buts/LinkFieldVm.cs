using CommunityToolkit.Mvvm.Input;
using Utils.WPF.Buttons;

namespace Comp_v4.TableWindows.SupplierOrders.Form.Vm.Buts;

public partial class ContractLinkFieldVm : LinkFieldVm
{
    public ContractLinkFieldVm() {
        FieldTitle = "Путь к файлу договора: ";
    }
}

public partial class InvoiceLinkFieldVm : LinkFieldVm
{
    public InvoiceLinkFieldVm() {
        FieldTitle = "Путь к файлу счёта: ";
    }
}

public abstract partial class LinkFieldVm : BaseButtonAdvanced
{
    protected string _url;
    protected string _fieldTitle;

    public string Url {
        get => _url;
        set {
            if (value == _url) return;
            _url = value;
            OnPropertyChanged();
        }
    }

    public string FieldTitle {
        get => _fieldTitle;
        set {
            if (value == _fieldTitle) return;
            _fieldTitle = value;
            OnPropertyChanged();
        }
    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    public async Task SetLink() {
        await base.OnClickAsync();
    } 

    public override void NotifyCanExecute() {
        SetLinkCommand.NotifyCanExecuteChanged();
    }
}