using Comp_v4.TableWindows.TypeSizes.Entities;
using Comp.ModelData.TechnicalItems;
using WPF.Services;
using WPF.Services.Validation;
using WPF.Templates.TableWindow.v1.Operations.Actions;

namespace Comp_v4.TableWindows.TypeSizes;

public class InstallerTypeSizesTable : AbstractInstaller
{
    protected readonly AreopagContainer _typeSizesNewItemWindowContainer = new();
    
    public InstallerTypeSizesTable() {
        new InstallerTypeSizesNewItemWindow().Install(_typeSizesNewItemWindowContainer);
    }
    
    protected override void InstallBindings(AreopagContainer container) {
        
        _typeSizesNewItemWindowContainer.Add<ValidatorBase<TypeSize>>()
                                        .AsScoped<AddTypeSizeWindow>()
                                        .UsingFactoryMethod(() => {
                                             var validator = container.Resolve<ValidatorBase<TypeSize>>();
                                             return validator;
                                         });
        
        container.Select<ActionStartAddingNewItem<TypeSizesTableWindow, TypeSize>>()
                 .OverrideTo<ActionAddingNewItem>();

        container.Add<AddTypeSizeWindowManager>()
                 .AsScoped<TypeSizesTableWindow>()
                 .UsingFactoryMethod(() => {
                      return new AddTypeSizeWindowManager(_typeSizesNewItemWindowContainer);
                  })
                 .EnforceInstantiateOnBegin();

        container.Add<NewItemCreateHandler>()
                 .AsScoped<TypeSizesTableWindow>()
                 .EnforceInstantiateOnBegin();
    }
}