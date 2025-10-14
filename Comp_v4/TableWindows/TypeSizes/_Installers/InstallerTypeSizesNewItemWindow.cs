using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows.TypeSizes.Vm;
using Comp_v4.TableWindows.TypeSizes.Vm.Buttons;
using Comp.ModelData;
using Comp.ModelData.TechnicalItems;
using WPF.Services;

namespace Comp_v4.TableWindows.TypeSizes;

public class InstallerTypeSizesNewItemWindow : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<TypeSize>().AsScoped<AddTypeSizeWindow>();
        container.Add<IImageOwner>().AsScoped<AddTypeSizeWindow>();
        
        container.Add<ImageFieldVmBase>().To<TsImageFieldVm>().AsScoped<AddTypeSizeWindow>();
        container.Add<SelectImageAction>().AsScoped<AddTypeSizeWindow>().EnforceInstantiateOnBegin();
        container.Add<ClearImageAction>().AsScoped<AddTypeSizeWindow>().EnforceInstantiateOnBegin();
        container.Add<OpenImageAction>().AsScoped<AddTypeSizeWindow>().EnforceInstantiateOnBegin();

        container.Add<ActionSaveNewItemForm>().AsScoped<AddTypeSizeWindow>();
        container.Add<ButtonSaveNewItemForm>().AsScoped<AddTypeSizeWindow>();

        container.Add<AddTypeSizeWindow>().AsTransient();
    }
}