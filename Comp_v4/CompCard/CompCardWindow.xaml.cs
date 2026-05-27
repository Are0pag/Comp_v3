using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Comp_v4._Installers;
using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Comp.ModelData;
using Utils.EventBus;
using Component = Comp.ModelData.Comp.Component;

namespace Comp_v4.CompCard;

public partial class CompCardWindow : Window, IDisposable, IRuntimeParamsResolver<Component>, IRuntimeParamsResolver<IImageOwner>, IRuntimeParamsResolver<CompCardWindow>
{
    protected readonly Component _component;
    
    public CompCardWindow(CdFieldVm cdFieldVm, ManFieldVm manFieldVm, MuFieldVm muFieldVm, TsFieldVm tsFieldVm, GpsFieldVm gpsFieldVm,
                          AnalogsFieldButtonVm analogsFieldButtonVm, AnalogsFieldVm analogsFieldVm,
                          Component component,
                          CardCopmEditController ec,
                          SaveCompButtonVm saveCompButtonVm) {
        InitializeComponent();
        
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
        _component = component;

        AnalogsFieldGrid.DataContext = analogsFieldVm;
        AnalogsFieldButton.DataContext = analogsFieldButtonVm;
        
        UrlFieldControl.DataContext = ec.UrlFieldControl;
        UrlAlternativeFieldControl.DataContext = ec.UrlAlternativeFieldControl;
        FilePathFieldControl.DataContext = ec.FilePathFieldControl;
        
        CdField.DataContext = cdFieldVm;
        ManField.DataContext = manFieldVm;
        MuField.DataContext = muFieldVm;
        TsField.DataContext = tsFieldVm;
        GpsField.DataContext = gpsFieldVm;
        
        ImageFieldControl.DataContext = ec.ImageField;

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
        
        QrCodeDataFieldUc.DataContext = ec.QrCodeDataField;
        DescriptionFieldUc.DataContext = ec.DescriptionField;
        CommentsFieldUc.DataContext = ec.CommentsField;
        
        SaveComponentButton.DataContext = saveCompButtonVm;
        
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public Task ResolveRuntimeParams(IRuntimeParamsContainer<Component> container) {
        container.RuntimeParam = _component;
        return Task.CompletedTask;
    }

    public Task ResolveRuntimeParams(IRuntimeParamsContainer<IImageOwner> container) {
        container.RuntimeParam = _component;
        return Task.CompletedTask;
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<CompCardWindow> container) {
        container.RuntimeParam = this;
    }

    private void SavePlacement(object? s, CancelEventArgs e) => WindowSettings.SavePlacement(this, nameof(CompCardWindow));

    private void LoadPlacement(object? s, EventArgs e) => WindowSettings.LoadPlacement(this, nameof(CompCardWindow));
    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
    }
    
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
