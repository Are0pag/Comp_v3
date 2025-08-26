using System.Windows;
using System.Windows.Controls;

namespace Comp_WpfUserControlLibrary;

public partial class InputTextFieldUc : UserControl
{
    public InputTextFieldUc() {
        InitializeComponent();
    }
    private void ClearButton_OnClick(object sender, RoutedEventArgs e) {
        InputTextBox.Clear();
        InputTextBox.Focus();
    }
}