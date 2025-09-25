using System.Windows;
using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Commands.Filtering;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using WPF.Services;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.Validation;
using WPF.Templates;
using WPF.Templates.TableWindow.States;
using WPF.Templates.TableWindow.Vm;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4;

public abstract class TableWindowInstaller<Tw, T> : AbstractInstaller
    where Tw : Window
    where T : class, IDbEntity, new()
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<IRepository<T>>().To<ConditionalDesignationRepository>().AsSingleton();

        container.Add<ICommandFactory>()
                 .To<DataGridCommandFactory>()
                 .AsScoped<TargetWindow>()
                 .UsingFactoryMethod(() => new DataGridCommandFactory(container));

        container.Add<AddItemCommand<T>>().AsTransient().WithParameters(typeof(T));
        container.Add<UpdateItemCommand<T>>().AsTransient().WithParameters(typeof(T));
        container.Add<DeleteItemCommand<T>>().AsTransient().WithParameters(typeof(T));

        container.Add<CreateRawCommand<TargetWindow, T>>()
                 .AsTransient()
                 .WithParameters(typeof(ModuleContext<TargetWindow, T>));

        container.Add<FocusCellCommand<TargetWindow, T>>()
                 .AsTransient()
                 .WithParameters(typeof(T));

        container.Add<RememberCellCommand<TargetWindow, T>>()
                 .AsTransient()
                 .WithParameters(typeof(RememberCellCommand<TargetWindow, T>.Args));
        
        container.Add<RememberInputTextCommand<TargetWindow, T>>()
                 .AsTransient()
                 .WithParameters(typeof(DataGridBeginningEditEventArgs));

        container.Add<RememberSelectionCommand<TargetWindow, T>>()
                 .AsTransient()
                 .WithParameters(typeof(object));

        container.Add<RemoveItemCommand<TargetWindow, T>>()
                 .AsTransient()
                 .WithParameters(typeof(T));

        /*container.Add<CellChangeStateCommand<Tw, Cd>>()
                 .AsTransient()
                 .WithParameters(typeof(object));*/
        
        container.Add<ApplyFilterCommand<TargetWindow, T, FiltersVmBase>>()
                 .AsTransient()
                 .WithParameters(typeof(ApplyFilterCommand<TargetWindow, T, FiltersVmBase>.Args));
        
        

        container.Add<ValidatorBase<T>>().To<Validator>().AsScoped<TargetWindow>();
        
        container.Add<IPropertyValueRestoreService<T>>()
                 .To<DataGridPropertyRestoreService<T>>()
                 .AsTransient();
        
        container.Add<IDataGridCommandScheduler>().To<DataGridCommandScheduler>().AsScoped<TargetWindow>();
        container.Add<IFilter<T, FiltersVmBase>>().To<Filter>().AsScoped<TargetWindow>();

        container.Add<DataGridViewModel<T>>().AsScoped<TargetWindow>();
        container.Add<FiltersVmBase>().AsScoped<TargetWindow>();
        container.Add<ModuleContext<TargetWindow, T>>().AsScoped<TargetWindow>();

        container.Add<ActionStartAddingNewItem<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<ActionAddItem<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<ActionUpdateItem<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<ActionDeleteItem<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<ActionSave<TargetWindow, T>>().AsScoped<TargetWindow>();
        
        container.Add<CellStateAddItem<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<CellStateUpdate<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<CellStateIdle<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<Cell<TargetWindow, T>>()
                 .AsScoped<TargetWindow>()
                 .UsingFactoryMethod(() => {
                      var initialState = container.Resolve<CellStateIdle<TargetWindow, T>>();
                      
                      var states = new List<BaseCellState<TargetWindow, T>>() {
                          initialState,
                          container.Resolve<CellStateAddItem<TargetWindow, T>>(),
                          container.Resolve<CellStateUpdate<TargetWindow, T>>(),
                      };

                      return new Cell<TargetWindow, T>(states, initialState);
                  });

        container.Add<ButtonVmAddItem<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<ButtonVmSave<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<ButtonVmDeleteItem<TargetWindow, T>>().AsScoped<TargetWindow>();

        container.Add<ActionStackTracker>().AsScoped<TargetWindow>();
        container.Add<PersistenceManager<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<TableCommandBinder<TargetWindow, T>>().To<TableCommandBinderFilteringCompatible<TargetWindow, T>>().AsScoped<TargetWindow>();
        container.Add<ActionFilter<TargetWindow, T, FiltersVmBase>>().AsScoped<TargetWindow>();

        container.Add<TargetWindow>().AsTransient();
    }
}