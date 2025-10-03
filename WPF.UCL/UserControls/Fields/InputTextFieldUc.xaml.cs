using System.Windows;
using System.Windows.Controls;

namespace WPF.UCL.UserControls.Fields;

public partial class InputTextFieldUc : UserControl
{
    public InputTextFieldUc() {
        InitializeComponent();
    }
    
    public static readonly DependencyProperty FieldNameProperty = DependencyProperty.Register(
     nameof(FieldName),
     typeof(string),
     typeof(InputTextFieldWithButtonUc),
     new PropertyMetadata("Поле:")); // Значение по умолчанию

    public string FieldName
    {
        get => (string)GetValue(FieldNameProperty);
        set => SetValue(FieldNameProperty, value);
    }
    
    public TextBox GetInputTextBox() => InputTextBox;
    
    public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register(
         nameof(InputText),
         typeof(string),
         typeof(InputTextFieldUc),
         new FrameworkPropertyMetadata(string.Empty, OnInputTextChanged)
         );

    public string InputText {
        get => (string)GetValue(InputTextProperty);
        set => SetValue(InputTextProperty, value);
    }

    private static void OnInputTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        
    }
    
    private void ClearButton_OnClick(object sender, RoutedEventArgs e) {
        InputTextBox.Clear();
        InputTextBox.Focus();
    }
}