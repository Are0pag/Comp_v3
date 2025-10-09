using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard;

public partial class CompCardWindow : Window, IDisposable
{
    protected readonly Component _component;
    
    public CompCardWindow(CdFieldVm cdFieldVm, ManFieldVm manFieldVm, MuFieldVm muFieldVm, TsFieldVm tsFieldVm, 
                          Component component,
                          CardCopmEditController ec,
                          SaveCompButtonVm saveCompButtonVm) {
        InitializeComponent();
        _component = component;
        
        CdField.DataContext = cdFieldVm;
        ManField.DataContext = manFieldVm;
        MuField.DataContext = muFieldVm;
        TsField.DataContext = tsFieldVm;

        CategoryNameTextBlock.DataContext = component;

        MainParameterField.DataContext = ec.GPMainField;
        Param1Field.DataContext = ec.GP1Field;
        Param2Field.DataContext = ec.GP2Field;
        Param3Field.DataContext = ec.GP3Field;
        Param4Field.DataContext = ec.GP4Field;
        Param5Field.DataContext = ec.GP5Field;
        
        InitSimpleField(ec.NameField, component.Name, NameFieldUc.GetInputTextBox());
        NameFieldUc.DataContext = ec.NameField;
        
        InitSimpleField(ec.NomenclatureNumberField, component.NomenclatureNumber, NomenclatureNumberFieldUc.GetInputTextBox());
        NomenclatureNumberFieldUc.DataContext = ec.NomenclatureNumberField;
        
        InitSimpleField(ec.CatalogNumberField, component.CatalogNumber, CatalogNumberFieldUc.GetInputTextBox());
        CatalogNumberFieldUc.DataContext = ec.CatalogNumberField;
        
        InitSimpleField(ec.LabelingOptionsField, component.LabelingOptions, LabelingOptionsFieldUc.GetInputTextBox());
        LabelingOptionsFieldUc.DataContext = ec.LabelingOptionsField;
        
        InitSimpleField(ec.CodeOfElementField, component.CodeOfElement, CodeOfElementFieldUc.GetInputTextBox());
        CodeOfElementFieldUc.DataContext = ec.CodeOfElementField;
        
        SaveComponentButton.DataContext = saveCompButtonVm;
    }

    public void Dispose() { }
    
    private void InitSimpleField(BaseTextFieldVm baseFieldVm, string currentValue, TextBox textBox) {
        baseFieldVm.Value = currentValue;
        baseFieldVm.PropertyChanged += (sender, args) => {
            if (baseFieldVm.IsValid()) {
                textBox.Background = new SolidColorBrush(Color.FromRgb(200, 255, 200)); // Светло-зеленый
                textBox.BorderBrush = new SolidColorBrush(Colors.Green);
            }
            else {
                textBox.Background = new SolidColorBrush(Color.FromRgb(255, 200, 200)); // Светло-красный
                textBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        };
    }
}