using CL_Comp_ModelData.Comp.ForeingParams;
using CL_Comp_ModelData.Comp.GenericParams;
using CL_Comp_ModelData.Comp.StringParams;
using Utils.WPF;

namespace CL_Comp_ModelData.Comp;


public class Component : NotifyPropertyChanged 
{
    public Dictionary<ParameterSetItem, GenValues> GenParams = new();
    public UniqueParams UniqueParams;
    public CommonParams CommonParams;
    public DescriptiveParams DescriptiveParams;
    public ExternalParams ExternalParams;
    public SortingParams SortingParams;
    public TechnicalParams TechnicalParams;
    protected ParameterSetItem _parameterSet;
        
    #region Ids
        public int IdDataBase {get; set;} // Db
        public int CategoryId {
            get {
                return _category.IdDataBase;
            }
        }
        public int GenericParametersSetItemId { get; set; }
        public int MeasurementUnitId { get; set; }
        public int ManufacturerId { get; set; }
        public int ConditionalDesignationId { get; set; }
        public int TypeSizeId { get; set; }
        public int ElementStatusId { get; set; }

    #endregion
            
        public ParameterSetItem ParameterSet {
            get => _parameterSet;
            set {
                if (value == _parameterSet) return;
                _parameterSet = value;
                OnPropertyChanged();
            }
        }
}