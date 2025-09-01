using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPF.UCL.UserControls.Grids;

public partial class DynamicDataGrid : UserControl
{
    public DynamicDataGrid() {
        InitializeComponent();
    }
    
    public IEnumerable ItemsSource {   // Свойство для источника данных
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(
            nameof(ItemsSource), 
            typeof(IEnumerable), 
            typeof(DynamicDataGrid), 
            new PropertyMetadata(null, OnItemsSourceChanged)
        );

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is DynamicDataGrid control) {
            control.DynamicItemsControl.ItemsSource = e.NewValue as IEnumerable;
        }
    }
    
    public void GenerateColumns(List<string> columnNames) {  // Метод динамической генерации столбцов
        var template = DynamicItemsControl.ItemTemplate as DataTemplate;
        
        if (template?.LoadContent() is not Grid gridTemplate) return;
        
        gridTemplate.ColumnDefinitions.Clear();  // Очистка существующих столбцов
            
        foreach (var columnName in columnNames) { // Создание новых столбцов
            gridTemplate.ColumnDefinitions.Add(new ColumnDefinition {
                Width = new GridLength(1, GridUnitType.Star)
            });

            var textBlock = new TextBlock {
                Text = columnName,
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(5)
            };

            // Привязка данных
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(columnName) { 
                Mode = BindingMode.OneWay
            });

            Grid.SetColumn(textBlock, gridTemplate.ColumnDefinitions.Count - 1);
            gridTemplate.Children.Add(textBlock);
        }
    }
}