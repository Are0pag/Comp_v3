using System.Windows;
using Comp_v3.NomDict.View;

namespace Comp_v3.Front.NomDict.View;

public partial class NomDictWindow : Window
{
    public NomDictWindow(NomDictWindowVm nomDictWindowVm) {
        InitializeComponent();
        DataContext = nomDictWindowVm;
    }
}