using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.Db.Repositories.Concrete;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using WPF.Services;

namespace Comp_v4.Installers;

public class CompRepoInstaller : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<IRepository<Category>>().To<RepositoryCategory>().AsTransient();
        container.Add<IRepository<ConditionalDesignation>>().To<DbRepository<ConditionalDesignation>>().AsTransient();
        container.Add<IRepository<Component>>().To<RepositoryComponent>().AsTransient();
    }
}