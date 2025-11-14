using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows.TypeSizes.Entities.Form;
using Comp_v4.TableWindows.TypeSizes.Entities.Form.States;
using Comp_v4.TableWindows.TypeSizes.Vm;
using Comp_v4.TableWindows.TypeSizes.Vm.Buttons;
using Comp.ModelData;
using Comp.ModelData.TechnicalItems;
using DI;
using Templates.Common.Actions.Images;

namespace Comp_v4.TableWindows.TypeSizes;

public class InstallerTypeSizesNewItemWindow : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<TypeSize>().AsScoped<TsFormWindow>();
        container.Add<IImageOwner>().AsScoped<TsFormWindow>();
        
        container.Add<ImageFieldVmBase>().To<TsImageFieldVm>().AsScoped<TsFormWindow>();
        container.Add<SelectImageAction>().AsScoped<TsFormWindow>().EnforceInstantiateOnBegin();
        container.Add<ClearImageAction>().AsScoped<TsFormWindow>().EnforceInstantiateOnBegin();
        container.Add<OpenTsImageAction>().AsScoped<TsFormWindow>().EnforceInstantiateOnBegin();
        
        /*container.Add<ActionSaveNewTsForm>().AsScoped<AddTypeSizeWindow>();
        container.Add<ButtonSaveNewItemForm>().AsScoped<AddTypeSizeWindow>();*/
        
        /*container.Add<AddItemTsStateForm>().AsScoped<AddTypeSizeWindow>();
        container.Add<EditItemTsStateForm>().AsScoped<AddTypeSizeWindow>();
        container.Add<FormTs>().AsScoped<AddTypeSizeWindow>().EnforceInstantiateOnBegin();*/

        //container.Add<AddTypeSizeWindow>().AsTransient();
    }
}