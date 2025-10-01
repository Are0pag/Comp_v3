using System.Windows;
using Comp_v4.CompCard.Vm;

namespace Comp_v4.CompCard;

public partial class CompCardWindow : Window, IDisposable
{
    protected readonly CompCardVm _compCardVm;
    
    public CompCardWindow(CompCardVm compCardVm, CdFieldVm cdFieldVm, ManFieldVm manFieldVm, MuFieldVm muFieldVm, TsFieldVm tsFieldVm) {
        InitializeComponent();
        _compCardVm = compCardVm;
        DataContext = _compCardVm;
        CdField.DataContext = cdFieldVm;
        ManField.DataContext = manFieldVm;
        MuField.DataContext = muFieldVm;
        TsField.DataContext = tsFieldVm;
    }

    public void Dispose() { }
}