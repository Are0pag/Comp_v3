using System.Windows;
using Comp_v3.NomDict.Contracts;

namespace Comp_v3.NomDict.View;

public partial class NomDictWindow : Window
{
    public NomDictWindow(NomDictWindowVm nomDictWindowVm) {
        InitializeComponent();
        DataContext = nomDictWindowVm;
    }
}