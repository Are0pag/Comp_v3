using System.Windows.Controls;
using Comp_v4.Entities;
using Comp_v4.Operations.Commands;
using Comp_v4.Operations.Commands.Filtering;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Microsoft.EntityFrameworkCore;
using WPF.Services;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.Validation;
using WPF.Templates;
using WPF.Templates.TableWindow.States;
using WPF.Templates.TableWindow.Vm;
using WPF.Templates.TableWindow.Vm.Components;

namespace Comp_v4;

public class CdWindowInstaller : AbstractInstaller
{
    protected override void InstallBindings(AreopagContainer container) {
        container.Add<IRepository<ConditionalDesignation>>().To<ConditionalDesignationRepository>().AsSingleton();

        container.Add<ICommandFactory>()
                 .To<DataGridCommandFactory>()
                 .AsScoped<TargetWindow>()
                 .UsingFactoryMethod(() => new DataGridCommandFactory(container));

        container.Add<AddItemCommand<ConditionalDesignation>>().AsTransient().WithParameters(typeof(ConditionalDesignation));
        container.Add<UpdateItemCommand<ConditionalDesignation>>().AsTransient().WithParameters(typeof(ConditionalDesignation));
        container.Add<DeleteItemCommand<ConditionalDesignation>>().AsTransient().WithParameters(typeof(ConditionalDesignation));

        container.Add<CreateRawCommand<TargetWindow, ConditionalDesignation>>()
                 .AsTransient()
                 .WithParameters(typeof(ModuleContext<TargetWindow, ConditionalDesignation>));

        container.Add<FocusCellCommand<TargetWindow, ConditionalDesignation>>()
                 .AsTransient()
                 .WithParameters(typeof(ConditionalDesignation));

        container.Add<RememberCellCommand<TargetWindow, ConditionalDesignation>>()
                 .AsTransient()
                 .WithParameters(typeof(RememberCellCommand<TargetWindow, ConditionalDesignation>.Args));
        
        container.Add<RememberInputTextCommand<TargetWindow, ConditionalDesignation>>()
                 .AsTransient()
                 .WithParameters(typeof(DataGridBeginningEditEventArgs));

        container.Add<RememberSelectionCommand<TargetWindow, ConditionalDesignation>>()
                 .AsTransient()
                 .WithParameters(typeof(object));

        container.Add<RemoveItemCommand<TargetWindow, ConditionalDesignation>>()
                 .AsTransient()
                 .WithParameters(typeof(ConditionalDesignation));

        /*container.Add<CellChangeStateCommand<Tw, Cd>>()
                 .AsTransient()
                 .WithParameters(typeof(object));*/
        
        container.Add<ApplyFilterCommand<TargetWindow, ConditionalDesignation, FiltersVmCd>>()
                 .AsTransient()
                 .WithParameters(typeof(ApplyFilterCommand<TargetWindow, ConditionalDesignation, FiltersVmCd>.Args));
        
        

        container.Add<ValidatorBase<ConditionalDesignation>>().To<Validator>().AsScoped<TargetWindow>();
        
        container.Add<IPropertyValueRestoreService<ConditionalDesignation>>()
                 .To<DataGridPropertyRestoreService<ConditionalDesignation>>()
                 .AsScoped<TargetWindow>();
        
        container.Add<IDataGridCommandScheduler>().To<DataGridCommandScheduler>().AsScoped<TargetWindow>();
        container.Add<IFilter<ConditionalDesignation, FiltersVmCd>>().To<Filter>().AsScoped<TargetWindow>();

        container.Add<DataGridViewModel<ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<FiltersVmCd>().AsScoped<TargetWindow>();
        container.Add<ModuleContext<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();

        container.Add<ActionStartAddingNewItem<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<ActionAddItem<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<ActionUpdateItem<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<ActionDeleteItem<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<ActionSave<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        
        container.Add<CellStateAddItem<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<CellStateUpdate<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<CellStateIdle<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<Cell<TargetWindow, ConditionalDesignation>>()
                 .AsScoped<TargetWindow>()
                 .UsingFactoryMethod(() => {
                      var initialState = container.Resolve<CellStateIdle<TargetWindow, ConditionalDesignation>>();
                      
                      var states = new List<BaseCellState<TargetWindow, ConditionalDesignation>>() {
                          initialState,
                          container.Resolve<CellStateAddItem<TargetWindow, ConditionalDesignation>>(),
                          container.Resolve<CellStateUpdate<TargetWindow, ConditionalDesignation>>(),
                      };

                      return new Cell<TargetWindow, ConditionalDesignation>(states, initialState);
                  });

        container.Add<ButtonVmAddItem<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<ButtonVmSave<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<ButtonVmDeleteItem<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();

        container.Add<ActionStackTracker>().AsScoped<TargetWindow>();
        container.Add<PersistenceManager<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<TableCommandBinder<TargetWindow, ConditionalDesignation>>().To<TableCommandBinderFilteringCompatible<TargetWindow, ConditionalDesignation>>().AsScoped<TargetWindow>();
        container.Add<ActionFilter<TargetWindow, ConditionalDesignation, FiltersVmCd>>().AsScoped<TargetWindow>();

        container.Add<TargetWindow>().AsTransient();
    }
}