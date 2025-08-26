using CL_Comp_ModelData.Comp.ForeingParams;
using CL_Comp_ModelData.Comp.GenericParams;
using CL_Comp_ModelData.Comp.StringParams;
using Utils.WPF.Mvvm;

namespace CL_Comp_ModelData.Comp;


public class Component : NotifyPropertyChanged 
{
    public Dictionary<GenKeys, GenValues> GenParams = new();
    public UniqueParams UniqueParams { get; set; }
    public CommonParams CommonParams { get; set; }
    public DescriptiveParams DescriptiveParams { get; set; }
    public ExternalParams ExternalParams { get; set; }
    public SortingParams SortingParams { get; set; }
    public TechnicalParams TechnicalParams { get; set; }

}