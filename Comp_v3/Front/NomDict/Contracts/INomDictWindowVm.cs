using Utils.WPF.Mvvm;

namespace Comp_v3.NomDict.Contracts;

public interface INomDictWindowVm
{
    ObservableModelProperty<string> NomDictViewModelProperty { get; set; }
}