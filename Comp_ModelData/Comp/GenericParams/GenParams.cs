using Utils.WPF.Mvvm;

namespace CL_Comp_ModelData.Comp.GenericParams;

public class GenParams
{
    public int Id { get; set; } 
    /*public string Alias  { get; set; }
    public (string, string) MainParam { get; set; }
    public (string, string)[] AdditionalParams { get; set; } = new (string, string)[4];*/
    
    public ObservableModelProperty<string> Alias { get; set; } = new();
    public ObservableModelProperty<(string KeyName, string Value)> MainParam { get; set; } = new();
    public ObservableModelProperty<(string KeyName, string Value)[]> AdditionalParams { get; set; } = new () {
        Value = new (string KeyName, string Value)[4] // Явно создаем массив нужного типа и размера
    };
}