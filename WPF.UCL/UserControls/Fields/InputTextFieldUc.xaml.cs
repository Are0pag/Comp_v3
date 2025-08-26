using System.Windows;
using System.Windows.Controls;

namespace WPF.UCL.UserControls.Fields;

public partial class InputTextFieldUc : UserControl
{
    public InputTextFieldUc() {
        InitializeComponent();
    }
    
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