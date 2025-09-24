using System.Windows;

namespace Comp_v4.CompCard;

public partial class CompCardWindow : Window
{
    protected readonly CompCardVm _compCardVm;
    
    public CompCardWindow(CompCardVm compCardVm) {
        InitializeComponent();
        _compCardVm = compCardVm;
        DataContext = _compCardVm;
    }
}