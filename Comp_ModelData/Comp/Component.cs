using Comp.ModelData.Comp.ForeingParams;
using Comp.ModelData.Comp.GenericParams;
using Comp.ModelData.Comp.StringParams;
using Comp.ModelData.TechnicalItems;
using Utils.WPF.Mvvm;

namespace Comp.ModelData.Comp;


public class Component : NotifyPropertyChanged 
{
    public Component() { }
    
    //public Dictionary<GenKeys, GenValues> GenParams = new();
    public UniqueParams UniqueParams { get; set; }
    public CommonParams CommonParams { get; set; }
    public DescriptiveParams DescriptiveParams { get; set; }
    public ExternalParams ExternalParams { get; set; }
    public SortingParams SortingParams { get; set; }
    //public TechnicalParams TechnicalParams { get; set; }

    public ConditionalDesignation ConditionalDesignation { get; set; }
    public Manufacturer Manufacturer { get; set; }
    public MeasurementUnit MeasurementUnit { get; set; }
}