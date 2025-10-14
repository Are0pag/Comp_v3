using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows.TypeSizes.Vm;
using Comp_v4.TableWindows.TypeSizes.Vm.Buttons;
using Comp.ModelData;
using Comp.ModelData.TechnicalItems;
using WPF.Services;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

namespace Comp_v4.TableWindows.TypeSizes;

public class InstallerTypeSizesTable : AbstractInstaller
{
    protected readonly AreopagContainer _typeSizesNewItemWindowContainer = new();
    
    public InstallerTypeSizesTable() {
        new InstallerTypeSizesNewItemWindow().Install(_typeSizesNewItemWindowContainer);
    }
    
    protected override void InstallBindings(AreopagContainer container) {
        //container.Add<IRepository<TypeSize>>().To<DbRepository<TypeSize>>().AsScoped<TypeSizesTableWindow>(); - регистрируется в базовом инсталлере
        //container.Add<DataGridViewModel<TypeSize>>().AsScoped<TypeSizesTableWindow>(); - регистрируется в базовом инсталлере
        //container.Add<FiltersVmBase>().AsScoped<TypeSizesTableWindow>(); - регистрируется в базовом инсталлере

        container.Select<ActionStartAddingNewItem<TypeSizesTableWindow, TypeSize>>()
                 .OverrideTo<ActionAddingNewItem>();
        
        /*container.Select<ButtonVmAddItem<TypeSizesTableWindow, TypeSize>>()
                 .OverrideTo<ButtonVmAddItemWithWindow>();*/

        container.Add<AddTypeSizeWindowManager>().AsScoped<TypeSizesTableWindow>().UsingFactoryMethod(() => {
            return new AddTypeSizeWindowManager(_typeSizesNewItemWindowContainer);
        });
    }
}

public class InstallerTypeSizesNewItemWindow : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<TypeSize>().AsScoped<AddTypeSizeWindow>();
        container.Add<IImageOwner>().AsScoped<AddTypeSizeWindow>();
        
        container.Add<ImageFieldVmBase>().To<TsImageFieldVm>().AsScoped<AddTypeSizeWindow>();
        container.Add<SelectImageAction>().AsScoped<AddTypeSizeWindow>();
        container.Add<ClearImageAction>().AsScoped<AddTypeSizeWindow>();
        container.Add<OpenImageAction>().AsScoped<AddTypeSizeWindow>();
        

        container.Add<AddTypeSizeWindow>().AsTransient();
    }
}