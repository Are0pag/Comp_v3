using Utils.WPF;

namespace Component_v1.Model.CompBataBase;

public class CardComp : NotifyPropertyChanged 
{

    #region SimplePrivateFields
        protected string _name;
        protected string _nomenclatureNumber;
        protected string _catalogNumber;
        protected string _labelingOptions;
        protected string _codeElement;
        protected string _qrCodeData;
        protected string _componentDescription;
        protected string _comment;
        protected string _imagePath;
    #endregion
    
    #region GenericFieldsPrivateMembers
        protected ParameterSetItem _parameterSet;
        protected string _genValueMain;
        protected string _genValue0;
        protected string _genValue1;
        protected string _genValue2;
        protected string _genValue3;
        protected string _genValue4;
    #endregion

    #region ForeingDataPrivateFields
        protected Category _category;
        protected MeasurementUnit _measurementUnit;
        protected Manufacturer _manufacturer;
        protected ConditionalDesignation _conditionalDesignation;
        protected TypeSize _typeSize;
        protected ElementStatus _elementStatus;
    #endregion


    
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


    #region SimpleFieldsProperties
    // (проверено)
        public string Name {
            get => _name;
            set {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        public string NomenclatureNumber {
            get => _nomenclatureNumber;
            set {
                if (value == _nomenclatureNumber) return;
                _nomenclatureNumber = value;
                OnPropertyChanged();
            }
        }
        public string CatalogNumber {
            get => _catalogNumber;
            set {
                if (value == _catalogNumber) return;
                _catalogNumber = value;
                OnPropertyChanged();
            }
        }
        public string LabelingOptions {
            get => _labelingOptions;
            set {
                if (value == _labelingOptions) return;
                _labelingOptions = value;
                OnPropertyChanged();
            }
        }
        public string CodeElement {
            get => _codeElement;
            set {
                if (value == _codeElement) return;
                _codeElement = value;
                OnPropertyChanged();
            }
        }
        public string QrCodeData {
            get => _qrCodeData;
            set {
                if (value == _qrCodeData) return;
                _qrCodeData = value;
                OnPropertyChanged();
            }
        }
        public string ComponentDescription {
            get => _componentDescription;
            set {
                if (value == _componentDescription) return;
                _componentDescription = value;
                OnPropertyChanged();
            }
        }
        public string Comment {
            get => _comment;
            set {
                if (value == _comment) return;
                _comment = value;
                OnPropertyChanged();
            }
        }
    
    #endregion
    
    public string CategoryName => _category?.Name;
    public string ImagePath {
        get => _imagePath;
        set {
            if (value == _imagePath) return;
            _imagePath = value;
            OnPropertyChanged();
        }
    }
    
    #region GenericFieldsProperties

        public ParameterSetItem ParameterSet {
            get => _parameterSet;
            set {
                if (value == _parameterSet) return;
                _parameterSet = value;
                OnPropertyChanged();
            }
        }

        public string GenValueMain {
            get => _genValueMain;
            set {
                if (value == _genValueMain) return;
                _genValueMain = value;
                OnPropertyChanged();
            }
        }

        public string GenValue0 {
            get => _genValue0;
            set {
                if (value == _genValue0) return;
                _genValue0 = value;
                OnPropertyChanged();
            }
        }
        
        public string GenValue1 {
            get => _genValue1;
            set {
                if (value == _genValue1) return;
                _genValue1 = value;
                OnPropertyChanged();
            }
        }

        public string GenValue2 {
            get => _genValue2;
            set {
                if (value == _genValue2) return;
                _genValue2 = value;
                OnPropertyChanged();
            }
        }

        public string GenValue3 {
            get => _genValue3;
            set {
                if (value == _genValue3) return;
                _genValue3 = value;
                OnPropertyChanged();
            }
        }

        public string GenValue4 {
            get => _genValue4;
            set {
                if (value == _genValue4) return;
                _genValue4 = value;
                OnPropertyChanged();
            }
        }

    #endregion

    #region ForeingDataProperties

        public Category Category {
            get => _category;
            set {
                if (value == _category) return;
                _category = value;
                OnPropertyChanged();
            }
        }
        public MeasurementUnit MeasurementUnit {
            get => _measurementUnit;
            set {
                if (value == _measurementUnit) return;
                _measurementUnit = value;
                OnPropertyChanged();
            }
        }
        public Manufacturer Manufacturer {
            get => _manufacturer;
            set {
                if (value == _manufacturer) return;
                _manufacturer = value;
                OnPropertyChanged();
            }
        }
        public ConditionalDesignation ConditionalDesignation {
            get => _conditionalDesignation;
            set {
                if (value == _conditionalDesignation) return;
                _conditionalDesignation = value;
                OnPropertyChanged();
                OnPropertyChanged("ConditionalDesignationText");
            }
        }
        public TypeSize TypeSize {
            get => _typeSize;
            set {
                if (value == _typeSize) return;
                _typeSize = value;
                OnPropertyChanged();
            }
        }
        public ElementStatus ElementStatus {
            get => _elementStatus;
            set {
                if (value == _elementStatus) return;
                _elementStatus = value;
                OnPropertyChanged();
            }
        }

    #endregion
}