using System.Windows;
using Comp_v4.CompCard.Entities;
using Comp_v4.TableWindows;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Microsoft.Extensions.DependencyInjection;
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

namespace Comp_v4.CompCard._Installers;

public static class TableWindowIntallerExt
{
    public static void RegisterTableWindows<Tw, T, TValidator, TFilter>(this IServiceCollection services) 
        where Tw : Window, IDisposable
        where T : class, IDbEntity, new()
        where TValidator : ValidatorBase<T>
        where TFilter : IFilter<T, FiltersVmBase>
    {
        services.AddSingleton<ICommandFactory, DataGridCommandFactory>();
        
        services.AddSingleton<IPropertyValueRestoreService<T>, DataGridPropertyRestoreService<T>>();
        services.AddSingleton<IDataGridCommandScheduler, DataGridCommandScheduler>();
        
        services.AddSingleton<ValidatorBase<T>, TValidator>();
        //services.AddSingleton<IFilter<T, FiltersVmBase>, TFilter>();
        services.AddSingleton<DataGridViewModel<T>>();
        services.AddSingleton<FiltersVmBase>();
        services.AddSingleton<ModuleContext<Tw, T>>();
        
        services.AddSingleton<ActionStartAddingNewItem<Tw, T>>();
        services.AddSingleton<ActionAddItem<Tw, T>>();
        services.AddSingleton<ActionUpdateItem<Tw, T>>();
        services.AddSingleton<ActionDeleteItem<Tw, T>>();
        services.AddSingleton<ActionSave<Tw, T>>();
        
        services.AddSingleton<CellStateAddItem<Tw, T>>();
        services.AddSingleton<CellStateUpdate<Tw, T>>();
        services.AddSingleton<CellStateIdle<Tw, T>>();
        services.AddSingleton<Cell<Tw, T>>(provider => {
            var initialState = provider.GetRequiredService<CellStateIdle<Tw, T>>();
                      
            var states = new List<BaseCellState<Tw, T>>() {
                initialState,
                provider.GetRequiredService<CellStateAddItem<Tw, T>>(),
                provider.GetRequiredService<CellStateUpdate<Tw, T>>(),
            };

            return new Cell<Tw, T>(states, initialState);
        });
        
        services.AddSingleton<ButtonVmAddItem<Tw, T>>();
        services.AddSingleton<ButtonVmSave<Tw, T>>();
        services.AddSingleton<ButtonVmDeleteItem<Tw, T>>();
        
        services.AddSingleton<ActionStackTracker>();
        services.AddSingleton<PersistenceManager<Tw, T>>();
        services.AddSingleton<TableCommandBinder<Tw, T>, TableCommandBinderExternalSelectionCompatible<Tw, T>>();
        services.AddSingleton<ActionFilter<Tw, T, FiltersVmBase>>();
        
        services.AddTransient<Tw>();
    }
}