using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard.Entities.Validation;

public class CardCopmEditController
{
    protected readonly List<BaseTextFieldVm> _requaredFieldViewModels;
    
    public NameFieldVm NameField { get; }
    public NomenclatureNumberFieldVm NomenclatureNumberField { get; }
    
    public CatalogNumberFieldVm CatalogNumberField { get; }
    public LabelingOptionsFieldVm LabelingOptionsField { get; }
    public CodeOfElementFieldVm CodeOfElementField { get; }
    
    public UrlFieldControlVm UrlFieldControl { get; }
    public UrlAlternativeFieldControlVm UrlAlternativeFieldControl { get; }
    public FilePathFieldControlVm FilePathFieldControl { get; }
    
    public QrCodeDataFieldVm QrCodeDataField { get; }
    public DescriptionFieldVm DescriptionField { get; }
    public CommentsFieldVm CommentsField { get; }
    
    public ImageFieldVmBase ImageField { get; }
    
    public gpMainFieldVm GPMainField { get; }
    public gp1FieldVm GP1Field { get; }
    public gp2FieldVm GP2Field { get; }
    public gp3FieldVm GP3Field { get; }
    public gp4FieldVm GP4Field { get; }
    public gp5FieldVm GP5Field { get; }

    public CardCopmEditController(NameFieldVm nameFieldVm, 
                                  NomenclatureNumberFieldVm nomenclatureNumberFieldVm, 
                                  CatalogNumberFieldVm catalogNumberField, 
                                  LabelingOptionsFieldVm labelingOptionsField, 
                                  CodeOfElementFieldVm codeOfElementField, 
                                  gpMainFieldVm gpMainField, gp1FieldVm gp1Field, gp2FieldVm gp2Field, gp3FieldVm gp3Field, gp4FieldVm gp4Field, gp5FieldVm gp5Field, 
                                  QrCodeDataFieldVm qrCodeDataField, DescriptionFieldVm descriptionField, CommentsFieldVm commentsField, UrlFieldControlVm urlFieldControl,
                                  UrlAlternativeFieldControlVm urlAlternativeFieldControl, FilePathFieldControlVm filePathFieldControl, 
                                  ImageFieldVmBase imageField) {
        _requaredFieldViewModels = new List<BaseTextFieldVm>() {
            nameFieldVm, nomenclatureNumberFieldVm
        };
        
        NameField = nameFieldVm;
        NomenclatureNumberField = nomenclatureNumberFieldVm;
        CatalogNumberField = catalogNumberField;
        LabelingOptionsField = labelingOptionsField;
        CodeOfElementField = codeOfElementField;
        GPMainField = gpMainField;
        GP1Field = gp1Field;
        GP2Field = gp2Field;
        GP3Field = gp3Field;
        GP4Field = gp4Field;
        GP5Field = gp5Field;
        QrCodeDataField = qrCodeDataField;
        DescriptionField = descriptionField;
        CommentsField = commentsField;
        UrlFieldControl = urlFieldControl;
        UrlAlternativeFieldControl = urlAlternativeFieldControl;
        FilePathFieldControl = filePathFieldControl;
        ImageField = imageField;
    }

    public bool IsValid() {
        return _requaredFieldViewModels.All(field => field.IsValid());
    }

    public Component ApplyEdits(Component card) {
        card.Name = NameField.Value;
        card.NomenclatureNumber = NomenclatureNumberField.Value;
        
        if (CatalogNumberField.IsValid())
            card.CatalogNumber = CatalogNumberField.Value;
        if (LabelingOptionsField.IsValid())
            card.LabelingOptions = LabelingOptionsField.Value;
        if (CodeOfElementField.IsValid())
            card.CodeOfElement = CodeOfElementField.Value;

        card.Url = UrlFieldControl.Url;
        card.UrlAlternative = UrlAlternativeFieldControl.Url;
        card.FilePath = FilePathFieldControl.Url;
        
        card.QrCodeData = QrCodeDataField.Value;
        card.Description = DescriptionField.Value;
        card.Comments = CommentsField.Value;
        
        card.GpMain = GPMainField.Value;
        card.Gp1 = GP1Field.Value;
        card.Gp2 = GP2Field.Value;
        card.Gp3 = GP3Field.Value;
        card.Gp4 = GP4Field.Value;
        card.Gp5 = GP5Field.Value;
        return card;
    }
}
