using System.Windows;

namespace Tests.Castle.Entities.Ui;

public partial class FloatingWindow : Window, IDisposable
{
    //protected readonly DogInt _dogInt;
    protected ViewModel _viewModel;
    
    public FloatingWindow(ViewModel viewModel) {
        _viewModel = viewModel;
        InitializeComponent();
    }

    public void Dispose() {
        
    }
}