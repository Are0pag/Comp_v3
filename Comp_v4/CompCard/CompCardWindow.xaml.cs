using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard;

public partial class CompCardWindow : Window, IDisposable
{
    protected readonly Component _component;
    
    public CompCardWindow(CdFieldVm cdFieldVm, ManFieldVm manFieldVm, MuFieldVm muFieldVm, TsFieldVm tsFieldVm, 
                          Component component,
                          NameFieldVm nameFieldVm, NomenclatureNumberFieldVm nomenclatureNumberFieldVm,
                          SaveCompButtonVm saveCompButtonVm) {
        InitializeComponent();
        _component = component;
        
        //cdFieldVm.Value = component.ConditionalDesignation.Designation;
        CdField.DataContext = cdFieldVm;
        /*cdFieldVm.PropertyChanged += (sender, args) => {
            
        };*/
        ManField.DataContext = manFieldVm;
        MuField.DataContext = muFieldVm;
        TsField.DataContext = tsFieldVm;

        CategoryNameTextBlock.DataContext = component;
        
        InitSimpleField(nameFieldVm, component.Name, NameFieldUc.GetInputTextBox());
        NameFieldUc.DataContext = nameFieldVm;
        
        InitSimpleField(nomenclatureNumberFieldVm, component.NomenclatureNumber, NomenclatureNumberFieldUc.GetInputTextBox());
        NomenclatureNumberFieldUc.DataContext = nomenclatureNumberFieldVm;
        
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