using System.Windows;
using Comp_v4.CompCard.Vm;
using Comp_v4.TableWindows.ConditionalDesignation;
using Comp_v4.TableWindows.GenericParametersSets;
using Comp_v4.TableWindows.Manufacturers;
using Comp_v4.TableWindows.MeasurementUnits;
using Comp_v4.TableWindows.TypeSizes;
using Comp.ModelData.TechnicalItems;
using Microsoft.Extensions.DependencyInjection;
using WPF.Templates.TableWindow.v1.Entities.InputHandlers;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.CompCard._Installers;

public static class CompCardButtonsToResolveTemplateWindowsInstallerExt
{
    public static void RegisterCardCompButtonsToResolveTemplateWindows(this IServiceCollection services) {
        services.AddSingleton<CdFieldVm>(provider => {
            return new CdFieldVm(() => {
                ResolveTableWindow<CondDesignTableWindow, ConditionalDesignation>(provider);
            });
        });
        
        services.AddSingleton<ManFieldVm>(provider => {
            return new ManFieldVm(() => {
                ResolveTableWindow<ManufacturersTableWindow, Manufacturer>(provider);
            });
        });
        
        services.AddSingleton<MuFieldVm>(provider => {
            return new MuFieldVm(() => {
                ResolveTableWindow<MeasurementUnitTableWindow, MeasurementUnit>(provider);
            });
        });
        
        services.AddSingleton<TsFieldVm>(provider => {
            return new TsFieldVm(() => {
                ResolveTableWindow<TypeSizesTableWindow, TypeSize>(provider);
                
            });
        });
        
        services.AddSingleton<GpsFieldVm>(provider => {
            return new GpsFieldVm(() => {
                ResolveTableWindow<GenericParametersSetsWindow, GenericParametersSet>(provider);
            });
        });
    }

    private static void ResolveTableWindow<TWindow, TData>(IServiceProvider serviceProvider)
        where TWindow : Window, IDisposable
        where TData : class, IDbEntity, new() 
    {
        var window = serviceProvider.GetRequiredService<TWindow>();
        serviceProvider.GetRequiredService<PersistenceManager<TWindow, TData>>();
        serviceProvider.GetRequiredService<TableCommandBinder<TWindow, TData>>();
        serviceProvider.GetRequiredService<ActionFilter<TWindow, TData, FiltersVmBase>>();
        window.Show();
    }
}