using System.Windows;
using Comp_v4.Entry.Vm.Buts;

namespace Comp_v4.Entry;

public partial class EntryWindow : Window, IDisposable
{
    public EntryWindow(NomDictButVm nomDictButVm, OrdersButVm ordersButVm) {
        InitializeComponent();
        NomWindowButton.DataContext = nomDictButVm;
        OrdersButton.DataContext = ordersButVm;
    }

    public void Dispose() {
        
    }
}