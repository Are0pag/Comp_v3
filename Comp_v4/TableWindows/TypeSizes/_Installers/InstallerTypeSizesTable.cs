using Comp_v4.TableWindows.TypeSizes.Entities;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.ModelData.TechnicalItems;
using WPF.Services;
using WPF.Services.Validation;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm;

namespace Comp_v4.TableWindows.TypeSizes;

public class InstallerTypeSizesTable : AbstractInstaller
{
    protected readonly AreopagContainer _formWindowContainer = new() {
        Description = "Container of TypeSize Form Window",
    };
    
    public InstallerTypeSizesTable() {
        new InstallerTypeSizesNewItemWindow().Install(_formWindowContainer);
    }
    
    protected override void InstallBindings(AreopagContainer container) {
        
        _formWindowContainer.Add<ValidatorBase<TypeSize>>()
                                        .AsScoped<AddTypeSizeWindow>()
                                        .UsingFactoryMethod(() => {
                                             var validator = container.Resolve<ValidatorBase<TypeSize>>();
                                             return validator;
                                         });

        _formWindowContainer.Add<IRepository<TypeSize>>()
                                        .To<DbRepository<TypeSize>>()
                                        .AsScoped<AddTypeSizeWindow>()
                                        .UsingFactoryMethod(() => {
                                             var repository = container.Resolve<IRepository<TypeSize>>();
                                             return repository;
                                         });

        _formWindowContainer.Add<DataGridViewModel<TypeSize>>()
                                        .AsScoped<AddTypeSizeWindow>()
                                        .UsingFactoryMethod(() => {
                                             var vm = container.Resolve<DataGridViewModel<TypeSize>>();
                                             return vm;
                                         });
        
        container.Select<ActionStartAddingNewItem<TypeSizesTableWindow, TypeSize>>()
                 .OverrideTo<ActionAddingNewItem>();

        container.Add<AddTypeSizeWindowManager>()
                 .AsScoped<TypeSizesTableWindow>()
                 .UsingFactoryMethod(() => {
                      return new AddTypeSizeWindowManager(_formWindowContainer);
                  })
                 .EnforceInstantiateOnBegin();

        container.Add<ActionOpenTsForm>()
                 .AsScoped<TypeSizesTableWindow>()
                 .EnforceInstantiateOnBegin();
    }
}