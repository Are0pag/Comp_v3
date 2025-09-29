using Comp_v4.NomDict.View;
using Comp_v4.NomDict.Vm;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using WPF.Services;

namespace Comp_v4.NomDict.Installers;

public class NomDictInstaller : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        /* предполагается что в данный контейнер уже зарегистрирован AppDbContext */
        container.Add<IRepository<Category>>().To<DbRepository<Category>>().AsTransient();
        container.Add<IRepository<Component>>().To<DbRepository<Component>>().AsTransient();

        container.Add<DataGridVm>().AsScoped<NomDictWindow>();
        container.Add<TreeViewVm>().AsScoped<NomDictWindow>();
        
        container.Add<NomDictWindow>().AsTransient();
    }
}