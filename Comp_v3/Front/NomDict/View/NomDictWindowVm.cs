using Comp_v3.NomDict.Contracts;
using Utils.WPF.Mvvm;

namespace Comp_v3.NomDict.View;

public class NomDictWindowVm : INomDictWindowVm
{
    public ObservableModelProperty<string> NomDictViewModelProperty { get; set; } = new ObservableModelProperty<string>() {Value = "Gerry Sqw bee"};
}