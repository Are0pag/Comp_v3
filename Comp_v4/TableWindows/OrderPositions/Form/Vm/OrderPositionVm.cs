using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData;
using Component = Comp.ModelData.Comp.Component;

namespace Comp_v4.TableWindows.OrderPositions.Form.Vm;

public class OrderPositionVm : ObservableObject, IDisposable
{
    private readonly OrderPosition _model;

    public OrderPositionVm(OrderPosition model) {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _model.PropertyChanged += OnModelPropertyChanged;
    }

#region Wrap

    public Component Position {
        get => _model.Position;
        set {
            SetProperty(_model.Position, value, _model, (m, v) => m.Position = v);
            _model.PositionId = value.Id;
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

#endregion

    protected void UpdateTotalCost() => _model.TotalCost = OrderQuantity * UnitPrice;

    protected void UpdateReceiveStatus() {
        var difference = OrderQuantity - ReceivedQuantity;
        _model.ReceiveStatus = difference switch {
            0                                  => ReceiveStatus.FullyReceived.ToString(),
            < 0                                => ReceiveStatus.OverReceived.ToString(),
            _ when difference == OrderQuantity => ReceiveStatus.NotReceived.ToString(),
            _                                  => ReceiveStatus.PartiallyReceived.ToString()
        };
    }

    protected void OnModelPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        OnPropertyChanged(e.PropertyName);
    }

    public void Dispose() {
        _model.PropertyChanged -= OnModelPropertyChanged;
    }
}