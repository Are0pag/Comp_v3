using System.Windows;
using Comp_v4.CompCard.Entities;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.ModelData.TechnicalItems;
using DI;
using Infrastructure.Command;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.Validation;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Entities.Cells;
using WPF.Templates.TableWindow.v1.Entities.Cells.States;
using WPF.Templates.TableWindow.v1.Entities.InputHandlers;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm;
using WPF.Templates.TableWindow.v1.Vm.Components;
using WPF.Templates.TableWindow.v1.Vm.Components.Buttons;

namespace Comp_v4.TableWindows;

public class TableWindowInstaller<Tw, T, TValidator, TFilter> : AbstractInstaller
    where Tw : Window, IDisposable
    where T : class, IDbEntity, new()
    where TValidator : ValidatorBase<T>
    where TFilter : IFilter<T, FiltersVmBase>
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<IRepository<T>>().To<DbRepository<T>>().AsTransient();

        container.Add<ICommandFactory>()
                 .To<DataGridCommandFactory>()
                 .AsScoped<Tw>()
                 .UsingFactoryMethod(() => new DataGridCommandFactory(container));

        /*container.Add<AddItemCommand<T>>().AsTransient().WithParameters(typeof(T));
        container.Add<UpdateItemCommand<T>>().AsTransient().WithParameters(typeof(T));
        container.Add<DeleteItemCommand<T>>().AsTransient().WithParameters(typeof(T));

        container.Add<CreateRawCommand<Tw, T>>()
                 .AsTransient()
                 .WithParameters(typeof(ModuleContext<Tw, T>));

        container.Add<FocusCellCommand<Tw, T>>()
                 .AsTransient()
                 .WithParameters(typeof(T));

        container.Add<RememberCellCommand<Tw, T>>()
                 .AsTransient()
                 .WithParameters(typeof(RememberCellCommand<Tw, T>.Args));
        
        container.Add<RememberInputTextCommand<Tw, T>>()
                 .AsTransient()
                 .WithParameters(typeof(DataGridBeginningEditEventArgs));

        container.Add<RememberSelectionCommand<Tw, T>>()
                 .AsTransient()
                 .WithParameters(typeof(object));

        container.Add<RemoveItemCommand<Tw, T>>()
                 .AsTransient()
                 .WithParameters(typeof(T));*/

        /*container.Add<CellChangeStateCommand<Tw, Cd>>()
                 .AsTransient()
                 .WithParameters(typeof(object));*/
        
        /*container.Add<ApplyFilterCommand<Tw, T, FiltersVmBase>>()
                 .AsTransient()
                 .WithParameters(typeof(ApplyFilterCommand<Tw, T, FiltersVmBase>.Args));*/


        container.Add<IPropertyValueRestoreService<T>>()
                 .To<DataGridPropertyRestoreService<T>>()
                 .AsTransient();

        container.Add<IDataGridCommandScheduler>().To<DataGridCommandScheduler>().AsScoped<Tw>();
        
        container.Add<ValidatorBase<T>>().To<TValidator>().AsScoped<Tw>();
        container.Add<IFilter<T, FiltersVmBase>>().To<TFilter>().AsScoped<Tw>();

        container.Add<DataGridViewModel<T>>().AsScoped<Tw>();
        container.Add<FiltersVmBase>().AsScoped<Tw>();
        container.Add<ModuleContext<Tw, T>>().AsScoped<Tw>();

        container.Add<ActionStartAddingNewItem<Tw, T>>().AsScoped<Tw>();
        container.Add<ActionAddItem<Tw, T>>().AsScoped<Tw>();
        container.Add<ActionUpdateItem<Tw, T>>().AsScoped<Tw>();
        container.Add<ActionDeleteItem<Tw, T>>().AsScoped<Tw>();
        container.Add<ActionSave<Tw, T>>().AsScoped<Tw>();
        
        container.Add<CellStateAddItem<Tw, T>>().AsScoped<Tw>();
        container.Add<CellStateUpdate<Tw, T>>().AsScoped<Tw>();
        container.Add<CellStateIdle<Tw, T>>().AsScoped<Tw>();
        container.Add<Cell<Tw, T>>()
                 .AsScoped<Tw>()
                 .UsingFactoryMethod(() => {
                      var initialState = container.Resolve<CellStateIdle<Tw, T>>();
                      
                      var states = new List<BaseCellState<Tw, T>>() {
                          initialState,
                          container.Resolve<CellStateAddItem<Tw, T>>(),
                          container.Resolve<CellStateUpdate<Tw, T>>(),
                      };

                      return new Cell<Tw, T>(states, initialState);
                  });

        container.Add<ButtonVmAddItem<Tw, T>>().AsScoped<Tw>();
        container.Add<ButtonVmSave<Tw, T>>().AsScoped<Tw>();
        container.Add<ButtonVmDeleteItem<Tw, T>>().AsScoped<Tw>();

        container.Add<ActionStackTracker>().AsScoped<Tw>();
        container.Add<PersistenceManager<Tw, T>>().AsScoped<Tw>();
        container.Add<TableCommandBinder<Tw, T>>().To<TableCommandBinderExternalSelectionCompatible<Tw, T>>().AsScoped<Tw>();
        container.Add<ActionFilter<Tw, T, FiltersVmBase>>().AsScoped<Tw>();

        container.Add<Tw>().AsTransient();
    }
}