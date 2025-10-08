using Comp_v4.Installers;
using Comp_v4.NomDict.Entities;
using Comp_v4.NomDict.Operations.Actions.Components;
using Comp_v4.NomDict.View;
using Comp_v4.NomDict.Vm;
using Comp_v4.NomDict.Vm.Buttons;
using Comp_v4.NomDict.Vm.Buttons.Components;
using WPF.Services;
using WPF.UCL;

namespace Comp_v4.NomDict.Installers;

public class NomDictInstaller : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        new CompRepoInstaller().Install(container);

        container.Add<DataGridVm>().AsScoped<NomDictWindow>();
        container.Add<TreeViewVm>().AsScoped<NomDictWindow>();

        

        container.Add<CategoryValidator>().AsScoped<NomDictWindow>();
        container.Add<OneValueWindow>().AsTransient();
        
        container.Add<AddNewCategoryButtonVm>().AsScoped<NomDictWindow>();
        container.Add<AddCategoryAction>().AsScoped<NomDictWindow>();

        container.Add<DeleteCategoryButtonVm>().AsScoped<NomDictWindow>();
        container.Add<DeleteCategoryAction>().AsScoped<NomDictWindow>();

        container.Add<UpdateCategoryNameButtonVm>().AsScoped<NomDictWindow>();
        container.Add<UpdateCategoryNameAction>().AsScoped<NomDictWindow>();

        container.Add<MoveCategoryAction>().AsScoped<NomDictWindow>();


        container.Add<AddCompButtonVm>().AsScoped<NomDictWindow>();
        container.Add<AddComponentAction>().AsScoped<NomDictWindow>();


        container.Add<NomDictWindow>().AsTransient();
    }
}