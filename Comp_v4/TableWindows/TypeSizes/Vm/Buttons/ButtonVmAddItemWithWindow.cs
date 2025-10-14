using Comp.ModelData.TechnicalItems;
using WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

namespace Comp_v4.TableWindows.TypeSizes.Vm.Buttons;

public partial class ButtonVmAddItemWithWindow : ButtonVmAddItem<TypeSizesTableWindow, TypeSize>
{
    public ButtonVmAddItemWithWindow(ActionAddingNewItem context) : base(context) {
    }
}