using System.Windows;
using Comp_v4.Entry.Vm.Buts;

namespace Comp_v4.Entry;

public partial class EntryWindow : Window, IDisposable
{
    public EntryWindow(NomDictButVm nomDictButVm) {
        InitializeComponent();
        NomWindowButton.DataContext = nomDictButVm;
    }

    public void Dispose() {
        
    }
}