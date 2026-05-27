using System.ComponentModel;
using System.Windows;
using Comp_v4._Installers;
using Comp_v4.Entry.Vm.Buts;
using Utils.EventBus;

namespace Comp_v4.Entry;

public partial class EntryWindow : Window, IDisposable, IRuntimeParamsResolver<EntryWindow>
{
    public EntryWindow(NomDictButVm nomDictButVm, OrdersButVm ordersButVm) {
        InitializeComponent();
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
        NomWindowButton.DataContext = nomDictButVm;
        OrdersButton.DataContext = ordersButVm;
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<EntryWindow> container) {
        container.RuntimeParam = this;
    }
    
    private void SavePlacement(object? s, CancelEventArgs e) => WindowSettings.SavePlacement(this, nameof(EntryWindow));

    private void LoadPlacement(object? s, EventArgs e) => WindowSettings.LoadPlacement(this, nameof(EntryWindow));

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
    }
}