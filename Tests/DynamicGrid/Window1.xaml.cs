using System.Collections.ObjectModel;
using System.Windows;

namespace Tests.DynamicGrid;

public partial class Window1 : Window
{
    public Window1(EmployeeViewModel viewModel) {
        InitializeComponent();
        DataContext = viewModel;
        
        // Динамический список с разными объектами
        var dynamicData = new ObservableCollection<dynamic> {
            new { Name = "Иван", Age = 30, City = "Москва" },
            new { Name = "Анна", Department = "IT", Salary = 5000 },
            new { Product = "Ноутбук", Price = 50000, Vendor = "Dell" }
        };

        // Установка данных
        DynamicGrid.GenerateColumns(dynamicData);
    }
}