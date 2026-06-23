using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Comp_v4._Installers;
using Comp_v4.TableWindows.Counterparties.Events;
using Comp_v4.TableWindows.Counterparties.Form.Vm;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp.ModelData;
using Utils.EventBus;

namespace Comp_v4.TableWindows.Counterparties;

public partial class CounterpartyFormWindow : Window, IDisposable, IRuntimeParamsResolver<Counterparty>, IRuntimeParamsResolver<CounterpartyFormWindow>
{
    private readonly Counterparty _counterparty;
    
    private readonly SaveCpFormButVm _saveCpFormButVm;
    private readonly CancelEditingCpButVm _cancelEditingCpButVm;
    public CounterpartyFormWindow(Counterparty counterparty, SaveCpFormButVm saveButVm, CounterpartyEnumsVm counterpartyEnumsVm, CancelEditingCpButVm cancelEditingCpButVm) {
        InitializeComponent();
        _counterparty = counterparty;
        WindowStartupLocation = WindowStartupLocation.Manual;
        SourceInitialized += LoadPlacement;
        Closing += SavePlacement;
        
        CounterpartyTypeComboBox.DataContext = counterpartyEnumsVm;
        
        _saveCpFormButVm = saveButVm;
        _cancelEditingCpButVm = cancelEditingCpButVm;
        counterparty.PropertyChanged += CounterpartyOnPropertyChanged;
        
        DataContext = counterparty;
        SaveButton.DataContext = saveButVm;
        CancelButton.DataContext = cancelEditingCpButVm;
        EventBus<IGlSubscriber>.Subscribe(this);
    }

    private void CounterpartyOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (sender is Counterparty counterparty && e.PropertyName == nameof(Counterparty.ShortName)) {
            _saveCpFormButVm.NotifyCanExecute();
            _cancelEditingCpButVm.NotifyCanExecute();
        }
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<Counterparty> container) {
        container.RuntimeParam = _counterparty;
    }

    public async Task ResolveRuntimeParams(IRuntimeParamsContainer<CounterpartyFormWindow> container) {
        container.RuntimeParam = this;
    }

    public void Dispose() {
        EventBus<IGlSubscriber>.Unsubscribe(this);
        SourceInitialized -= LoadPlacement;
        Closing -= SavePlacement;
        _counterparty.PropertyChanged -= CounterpartyOnPropertyChanged;
    }

    public Task Save(TaskCompletionSource<Counterparty> tcs, object? parameter = null) {
        Close();
        tcs.TrySetResult((Counterparty)parameter!);
        return Task.CompletedTask;
    }
    
    private void SavePlacement(object? s, CancelEventArgs e) => WindowPlaceSizeSettings.SavePlacement(this, GetType().ToString());
    private void LoadPlacement(object? s, EventArgs e) => WindowPlaceSizeSettings.LoadPlacement(this, GetType().ToString());

    private async void CounterpartyFormWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Escape:
                await _cancelEditingCpButVm.OnClickAsync();
                break;
        }
    }
}