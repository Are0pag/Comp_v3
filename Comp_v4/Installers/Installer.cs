using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Commands.Filtering;
using Comp.Db;
using Comp.Db.Contracts;
using Infrastructure.Command;
using Infrastructure.Command.Heterochromic;
using Microsoft.EntityFrameworkCore;
using WPF.Services.Validation;
using WPF.Templates;
using WPF.Templates.TableWindow.States;
using WPF.Templates.TableWindow.Vm;
using WPF.Templates.TableWindow.Vm.Components;

using Tw = Comp_v4.TargetWindow;
using Cd = Comp.ModelData.TechnicalItems.ConditionalDesignation;

namespace Comp_v4;

public class Installer: IWindsorInstaller
{
    protected readonly string _targetAssemblyName = "WPF.Templates.TableWindow.v1";

    public void Install(IWindsorContainer container, IConfigurationStore store) {
        container.Register(Component.For<AppDbContext>()
                                    .UsingFactoryMethod(kernel => {
                                         var options = new DbContextOptionsBuilder<AppDbContext>()
                                                      .UseSqlite(DbConfig.ConnectionString)
                                                      .Options;

                                         return new AppDbContext(options);
                                     })
                                    .LifestyleScoped());
        
        container.Register(Component.For<IRepository<Cd>>()
                                    .ImplementedBy<ConditionalDesignationRepository>()
                                    .LifestyleTransient());

        // потом подумать про освобождение ресурсов
        container.Register(
            Component.For<ICommandFactory>().ImplementedBy<WindsorCommandFactory>().LifestyleBoundTo<Tw>(),
            Component.For<ApplyFilterCommand<Tw, Cd, FiltersVmCd>>()
                     .DependsOn(Property.ForKey<ApplyFilterCommand<Tw, Cd, FiltersVmCd>.Args>())
                     .LifestyleBoundTo<Tw>()
            /*Classes.FromAssemblyNamed(_targetAssemblyName)
                   .BasedOn(typeof(DeferredCommandBase<>))
                   .WithServiceSelf()
                   .LifestyleBoundTo<Tw>()*/);
        
        

        
        
        container.Register(Component.For<System.Windows.Threading.Dispatcher>()
                                    .LifestyleSingleton());

        container.Register(Component.For<ValidatorBase<Cd>>().ImplementedBy<Validator>().LifestyleBoundTo<Tw>(),
                           Component.For<IDataGridCommandScheduler>().ImplementedBy<DataGridCommandScheduler>().LifestyleBoundTo<Tw>(),
                           Component.For<IFilter<Cd, FiltersVmCd>>().ImplementedBy<Filter>().LifestyleBoundTo<Tw>(),
                           
                           Component.For<DataGridViewModel<Cd>>().LifestyleBoundTo<Tw>(),
                           Component.For<FiltersVmCd>().LifestyleBoundTo<Tw>(),
                           
                           Component.For<ModuleContext<Tw, Cd>>().LifestyleBoundTo<Tw>(),
                           
                           Component.For<ActionStartAddingNewItem<Tw, Cd>>().LifestyleBoundTo<Tw>(),
                           Component.For<ActionAddItem<Tw, Cd>>().LifestyleBoundTo<Tw>(),
                           Component.For<ActionUpdateItem<Tw, Cd>>().LifestyleBoundTo<Tw>(),
                           Component.For<ActionDeleteItem<Tw, Cd>>().LifestyleBoundTo<Tw>(),
                           Component.For<ActionSave<Tw, Cd>>().LifestyleBoundTo<Tw>());

        container.Register( // Конкретные состояния
                           Component.For<CellStateIdle<Tw, Cd>>().LifestyleSingleton(),
                           Component.For<CellStateUpdate<Tw, Cd>>().LifestyleSingleton(),
                           Component.For<CellStateAddItem<Tw, Cd>>().LifestyleSingleton(),

                           // Каждое состояние также как BaseCellState
                           Component.For<BaseCellState<Tw, Cd>>().Named("IdleState")
                                    .ImplementedBy<CellStateIdle<Tw, Cd>>().LifestyleSingleton(),
                           Component.For<BaseCellState<Tw, Cd>>().Named("UpdateState")
                                    .ImplementedBy<CellStateUpdate<Tw, Cd>>().LifestyleSingleton(),
                           Component.For<BaseCellState<Tw, Cd>>().Named("AddItemState")
                                    .ImplementedBy<CellStateAddItem<Tw, Cd>>().LifestyleSingleton());
        
        container.Register(Component.For<Cell<Tw, Cd>>().UsingFactoryMethod(kernel => {
                               var states = kernel.ResolveAll<BaseCellState<Tw, Cd>>();
                               var initialState = kernel.Resolve<CellStateIdle<Tw, Cd>>(); // или найти по условию
                               return new Cell<Tw, Cd>(states, initialState);
                           }).LifestyleBoundTo<Tw>());

        container.Register(Component.For<ButtonVmAddItem<Tw, Cd>>().LifestyleBoundTo<Tw>());
        container.Register(Component.For<ButtonVmSave<Tw, Cd>>().LifestyleBoundTo<Tw>());
        container.Register(Component.For<ButtonVmDeleteItem<Tw, Cd>>().LifestyleBoundTo<Tw>());

        container.Register(
            Component.For<ActionStackTracker>().LifestyleBoundTo<Tw>(),
            Component.For<PersistenceManager<Tw, Cd>>().LifestyleBoundTo<Tw>(),
            Component.For<TableCommandBinder<Tw, Cd>>()
                     .ImplementedBy<TableCommandBinderFilteringCompatible<Tw, Cd>>().LifestyleBoundTo<Tw>(),
            Component.For<ActionFilter<Tw, Cd, FiltersVmCd>>().LifestyleBoundTo<Tw>(),
            Component.For<DummyWindowEventsHandler<Tw, Cd, FiltersVmCd>>().LifestyleBoundTo<Tw>());

        container.Register(Component.For<Tw>().LifestyleTransient());
    }
}