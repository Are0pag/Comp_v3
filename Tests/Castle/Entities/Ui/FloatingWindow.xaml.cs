using System.Windows;

namespace Tests.Castle.Entities.Ui;

public partial class FloatingWindow : Window
{
    protected readonly DogInt _dogInt;
    
    public FloatingWindow(DogInt dogInt) {
        InitializeComponent();
        _dogInt = dogInt;
    }
}