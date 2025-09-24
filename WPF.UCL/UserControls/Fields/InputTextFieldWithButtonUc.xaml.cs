using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.Input;

namespace WPF.UCL.UserControls.Fields;

public partial class InputTextFieldWithButtonUc : UserControl
{
    public InputTextFieldWithButtonUc() {
        InitializeComponent();
    }

    public static readonly DependencyProperty InputTextProperty = DependencyProperty.Register(
         nameof(InputText),
         typeof(string),
         typeof(InputTextFieldWithButtonUc),
         new FrameworkPropertyMetadata(string.Empty, OnInputTextChanged));

    public string InputText {
        get => (string)GetValue(InputTextProperty);
        set => SetValue(InputTextProperty, value);
    }

    public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.Register(
         nameof(ButtonContent),
         typeof(object),
         typeof(InputTextFieldWithButtonUc),
         new PropertyMetadata("⚡")); // Значок по умолчанию

    public object ButtonContent {
        get => GetValue(ButtonContentProperty);
        set => SetValue(ButtonContentProperty, value);
    }

    public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.Register(
         nameof(ButtonCommand),
         typeof(IRelayCommand),
         typeof(InputTextFieldWithButtonUc),
         new PropertyMetadata(null));

    public IRelayCommand ButtonCommand {
        get => (IRelayCommand)GetValue(ButtonCommandProperty);
        set => SetValue(ButtonCommandProperty, value);
    }

    public static readonly DependencyProperty ButtonCommandParameterProperty = DependencyProperty.Register(
         nameof(ButtonCommandParameter),
         typeof(object),
         typeof(InputTextFieldWithButtonUc),
         new PropertyMetadata(null));

    public object ButtonCommandParameter {
        get => GetValue(ButtonCommandParameterProperty);
        set => SetValue(ButtonCommandParameterProperty, value);
    }

    public static readonly DependencyProperty ButtonToolTipProperty = DependencyProperty.Register(
         nameof(ButtonToolTip),
         typeof(string),
         typeof(InputTextFieldWithButtonUc),
         new PropertyMetadata("Действие"));

    public string ButtonToolTip {
        get => (string)GetValue(ButtonToolTipProperty);
        set => SetValue(ButtonToolTipProperty, value);
    }

    private static void OnInputTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        // Можно добавить дополнительную логику при изменении текста
    }

    private void ClearButton_OnClick(object sender, RoutedEventArgs e) {
        InputText = string.Empty;
        InputTextBox.Focus();
    }
}