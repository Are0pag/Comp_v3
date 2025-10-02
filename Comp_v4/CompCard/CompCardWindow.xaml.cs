using System.Windows;
using Comp_v4.CompCard.Vm;
using Comp.ModelData.Comp;

namespace Comp_v4.CompCard;

public partial class CompCardWindow : Window, IDisposable
{
    protected readonly Component _component;
    
    public CompCardWindow(CdFieldVm cdFieldVm, ManFieldVm manFieldVm, MuFieldVm muFieldVm, TsFieldVm tsFieldVm, Component component,
                          NameFieldVm nameFieldVm, NomenclatureNumberFieldVm nomenclatureNumberFieldVm) {
        InitializeComponent();
        _component = component;
        
        //cdFieldVm.Value = component.ConditionalDesignation.Designation;
        CdField.DataContext = cdFieldVm;
        ManField.DataContext = manFieldVm;
        MuField.DataContext = muFieldVm;
        TsField.DataContext = tsFieldVm;
        
        nameFieldVm.Value = component.Name;
        NameFieldUc.DataContext = nameFieldVm;
        
        nomenclatureNumberFieldVm.Value = component.NomenclatureNumber;
        NomenclatureNumberFieldUc.DataContext = nomenclatureNumberFieldVm;
    }

    public void Dispose() { }
}