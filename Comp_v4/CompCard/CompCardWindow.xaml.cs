using System.Windows;
using Comp_v4.CompCard.Vm;

namespace Comp_v4.CompCard;

public partial class CompCardWindow : Window
{
    protected readonly CompCardVm _compCardVm;
    
    public CompCardWindow(CompCardVm compCardVm, CdFieldVm cdFieldVm, ManFieldVm manFieldVm) {
        InitializeComponent();
        _compCardVm = compCardVm;
        DataContext = _compCardVm;
        CdField.DataContext = cdFieldVm;
        ManField.DataContext = manFieldVm;
    }
}