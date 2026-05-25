using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPF.Services.Validation;

namespace WPF.UCL;

public partial class OneValueWindow : Window
{
    protected readonly Func<string, bool> _isValidCheck;
    protected readonly Func<string, Task<bool>>? _isValidCheckAsync;
    
    public OneValueWindow(string valueName, Func<string, bool> isValidCheck, string? textInInputField = null) {
        InitializeComponent();
        _isValidCheck = isValidCheck;
        ValueNameTextBlock.Text = valueName;
        if (textInInputField != null) 
            InputTextBox.Text = textInInputField;
        
        Loaded += (s, e) => {
            InputTextBox.Focus();
            Keyboard.Focus(InputTextBox);
        };
    }

    public OneValueWindow(string valueName, Func<string, Task<bool>> isValidCheckAsync, string? textInInputField = null) {
        InitializeComponent();
        _isValidCheckAsync = isValidCheckAsync;
        ValueNameTextBlock.Text = valueName;
        if (textInInputField != null) 
            InputTextBox.Text = textInInputField;
        
        Loaded += (s, e) => {
            InputTextBox.Focus();
            Keyboard.Focus(InputTextBox);
        };
    }
    
    public string GetCurrentValue() => InputTextBox.Text;

    protected virtual async void InputTextBox_OnTextChanged(object sender, TextChangedEventArgs e) {
        bool isValid = false;
        
        if (_isValidCheck != null) {
            isValid = _isValidCheck.Invoke(InputTextBox.Text);
        }
        else if (_isValidCheckAsync != null) {
            // Fixed: Use GetAwaiter().GetResult() instead of .Result to avoid deadlocks
            isValid = await _isValidCheckAsync(InputTextBox.Text);
        }
        
        if (isValid) {
            InputTextBox.Background = new SolidColorBrush(Color.FromRgb(200, 255, 200)); // Светло-зеленый
            InputTextBox.BorderBrush = new SolidColorBrush(Colors.Green);
        }
        else {
            InputTextBox.Background = new SolidColorBrush(Color.FromRgb(255, 200, 200)); // Светло-красный
            InputTextBox.BorderBrush = new SolidColorBrush(Colors.Red);
        }
    }

    protected virtual async void OneValueWindow_OnPreviewKeyDown(object sender, KeyEventArgs e) {
        switch (e.Key) {
            case Key.Enter:
                bool isValid = false;
                
                if (_isValidCheck != null) {
                    isValid = _isValidCheck.Invoke(InputTextBox.Text);
                }
                else if (_isValidCheckAsync != null) {
                    // Fixed: Use await instead of .Result to avoid deadlocks
                    isValid = await _isValidCheckAsync(InputTextBox.Text);
                }
                
                if (!isValid)
                    return;
                
                DialogResult = true;
                Dispatcher.BeginInvoke(new Action(() => {
                    Close();
                }));
                e.Handled = true;
                break;
        }
    }
}
