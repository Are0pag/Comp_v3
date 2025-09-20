using System.Windows;

namespace Tests.Castle.Entities.Ui;

public partial class FloatingWindow : Window
{
    protected readonly FloatingVm _floatingVm;
    
    public FloatingWindow(FloatingVm floatingVm) {
        InitializeComponent();
        _floatingVm = floatingVm;
    }
}