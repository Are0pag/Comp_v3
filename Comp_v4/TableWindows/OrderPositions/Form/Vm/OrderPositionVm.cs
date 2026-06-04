using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp_v4._Installers;
using Comp_v4.TableWindows.SupplierOrders.Events;
using Comp.ModelData;
using Utils.EventBus;
using Component = Comp.ModelData.Comp.Component;

namespace Comp_v4.TableWindows.OrderPositions.Form.Vm;

public class OrderPositionVm : ObservableObject, IDisposable
{
    protected readonly ReceiveStatusEnumVm _receiveStatusEnumVm;
    protected readonly OrderPosition _model;

    public OrderPositionVm(ReceiveStatusEnumVm receiveStatusEnumVm, OrderPosition model) {
        _receiveStatusEnumVm = receiveStatusEnumVm;
        _model = model;
        _model.PropertyChanged += OnModelPropertyChanged;
    }

#region Wrap

    public Component Position => _model.Position;

    public SupplierOrder RelatedSupplierOrder => _model.SupplierOrder;

    public ReceiveStatus ReceiveStatusEnumValue {
        get => _model.ReceiveStatusEnumValue;
        set {
            SetProperty(_model.ReceiveStatusEnumValue, value, _model, (m, v) => m.ReceiveStatusEnumValue = v);
        }
    }

    public int OrderQuantity {
        get => _model.OrderQuantity;
        set {
            SetProperty(_model.OrderQuantity, value, _model, (m, v) => m.OrderQuantity = v);
            UpdateReceiveStatus();
            UpdateTotalCost();
        }
    }

    public int ReceivedQuantity {
        get => _model.ReceivedQuantity;
        set {
            SetProperty(_model.ReceivedQuantity, value, _model, (m, v) => m.ReceivedQuantity = v);
            UpdateReceiveStatus();
        }
    }

    public decimal UnitPrice {
        get => _model.UnitPrice;
        set {
            SetProperty(_model.UnitPrice, value, _model, (m, v) => m.UnitPrice = v);
            UpdateTotalCost();
        }
    }

    public decimal TotalCost {
        get => _model.TotalCost;
        set {
            throw new InvalidOperationException(); // А вот без этого сеттера WPF хуй заработает хых
        }
    }

    public SupplierOrder SupplierOrder => _model.SupplierOrder;

#endregion

    protected void UpdateTotalCost() => _model.TotalCost = OrderQuantity * UnitPrice;

    protected void UpdateReceiveStatus() {
        var difference = OrderQuantity - ReceivedQuantity;
        _receiveStatusEnumVm.SelectedValue = difference switch {
            0                                  => ReceiveStatus.FullyReceived,
            < 0                                => ReceiveStatus.OverReceived,
            _ when difference == OrderQuantity => ReceiveStatus.NotReceived,
            _                                  => ReceiveStatus.PartiallyReceived
        };
        _model.ReceiveStatus = _receiveStatusEnumVm.SelectedValue.ToString();
    }

    protected void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        OnPropertyChanged(e.PropertyName);
        EventBus<ISupplierOrdersSubscriber>
           .RaiseEvent<ISoPropertyChangeHandler>(h => {
                h?.OnOrderPositionChanged();
            });
    }

    public void Dispose() {
        _model.PropertyChanged -= OnModelPropertyChanged;
    }
}