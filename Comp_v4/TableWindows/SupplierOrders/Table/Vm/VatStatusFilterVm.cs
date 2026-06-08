using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Comp.ModelData;

namespace Comp_v4.TableWindows.SupplierOrders.Table.Vm;

public enum OrderStatusFilter : byte
{
    [Description("Все")]
    All,
    [Description("Создан")]
    Created,
    [Description("Заказан")]
    Ordered,
    [Description("Получен")]
    Received,
    [Description("Архивный")]
    Archived
}

public class VatStatusFilterVm : ObservableObject
{
    protected ObservableCollection<SupplierOrder> _items;
    protected ObservableCollection<SupplierOrder> _filteredItems;
    
    public List<OrderStatusFilter> OrderStatusFilterValues { get; } = new() {
        OrderStatusFilter.All,
        OrderStatusFilter.Created,
        OrderStatusFilter.Ordered,
        OrderStatusFilter.Received,
        OrderStatusFilter.Archived
    };
    
    private OrderStatusFilter _selectedOrderStatusFilter = OrderStatusFilter.All;

    public void Init(ObservableCollection<SupplierOrder> items, ObservableCollection<SupplierOrder> filteredItems) {
        _items = items;
        _filteredItems = filteredItems;
    }

    public OrderStatusFilter SelectedOrderStatusFilter
    {
        get => _selectedOrderStatusFilter;
        set {
            _selectedOrderStatusFilter = value;
            OnPropertyChanged(nameof(SelectedOrderStatusFilter));
            ApplyFilter();
        }
    }

    public void ApplyFilter() {
        _filteredItems.Clear();

        if (SelectedOrderStatusFilter == OrderStatusFilter.All) {
            foreach (var item in _items) {
                _filteredItems.Add(item);
            }
        }
        else {
            var selectedName = SelectedOrderStatusFilter.ToString();

            // Превращаем эту строку в оригинальный бизнес-enum VatStatus
            var targetVatStatus = (OrderStatus)Enum.Parse(typeof(OrderStatus), selectedName);

            var query = _items.Where(item => item.OrderStatusEnumValue == targetVatStatus);
            foreach (var item in query) {
                _filteredItems.Add(item);
            }
        }
    }
}