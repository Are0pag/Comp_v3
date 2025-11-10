using System.Windows;
using Comp_v4.TableWindows;
using Comp.ModelData.TechnicalItems;
using Infrastructure.Command;
using Microsoft.Extensions.DependencyInjection;
using WPF.Services.UserActionsHandling.InputText;
using WPF.Services.Validation;
using WPF.Templates.TableWindow.v1.Entities;
using WPF.Templates.TableWindow.v1.Operations.Commands.Filtering;
using WPF.Templates.TableWindow.v1.Vm.Components;

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
        services.AddSingleton<IDataGridCommandScheduler, DataGridCommandScheduler>();
        services.AddSingleton<IDataGridCommandScheduler, DataGridCommandScheduler>();
        
    }
}