using Comp_v4.CompCard.Events;
using Comp.ModelData.Comp;
using Utils.EventBus;

namespace Comp_v4.CompCard.Vm.Buttons;

public abstract class LinkFieldControlVm : SetLinkButtonVm, ICompCardLoadedHandler
{
    private string _fieldTitle;
    private string _url = string.Empty;

    public LinkFieldControlVm() {
        EventBus<ICompCardSubscriber>.Subscribe(this);
    }

    public override void Dispose() {
        base.Dispose();
        EventBus<ICompCardSubscriber>.Unsubscribe(this);
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

    public abstract void OnCompCardLoaded(Component component);
}

public class UrlFieldControlVm : LinkFieldControlVm
{
    public UrlFieldControlVm() {
        FieldTitle = "Интернет ссылка: ";
    }

    public override void OnCompCardLoaded(Component component) {
        Url = component.Url;
    }
}

public class UrlAlternativeFieldControlVm : LinkFieldControlVm
{
    public UrlAlternativeFieldControlVm() {
        FieldTitle = "Альтернативная ссылка: ";
    }

    public override void OnCompCardLoaded(Component component) {
        Url = component.UrlAlternative;
    }
}

public class FilePathFieldControlVm : LinkFieldControlVm
{
    public FilePathFieldControlVm() {
        FieldTitle = "Файл спецификации: ";
    }

    public override void OnCompCardLoaded(Component component) {
        Url = component.FilePath;
    }
}

