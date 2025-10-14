using System.Windows;
using Comp_v4.CompCard.Vm;
using Comp.ModelData.TechnicalItems;

namespace Comp_v4.TableWindows.TypeSizes;

public partial class AddTypeSizeWindow : Window, IDisposable
{
    public AddTypeSizeWindow(ImageFieldVm imageFieldVm, TypeSize typeSize) {
        InitializeComponent();
        DataContext = typeSize;
        ImageFieldControl.DataContext = imageFieldVm;
    }

    public void Dispose() {
        
    }
}