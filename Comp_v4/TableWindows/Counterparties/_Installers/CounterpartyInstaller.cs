using Comp_v4.TableWindows.Counterparties.Form.Actions;
using Comp_v4.TableWindows.Counterparties.Form.Entities;
using Comp_v4.TableWindows.Counterparties.Form.Vm;
using Comp_v4.TableWindows.Counterparties.Form.Vm.Buts;
using Comp_v4.TableWindows.Counterparties.Table;
using Comp_v4.TableWindows.Counterparties.Table.Actions;
using Comp_v4.TableWindows.Counterparties.Table.Entities;
using Comp_v4.TableWindows.Counterparties.Table.Vm;
using Comp_v4.TableWindows.Counterparties.Table.Vm.But;
using Comp.ModelData;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.TableWindows.Counterparties._Installers;

public static class CounterpartyInstaller
{
    public static void RegisterCounterparties(this IServiceCollection services) {
        RegisterTable(services);
        RegisterForm(services);
    }

    private static void RegisterForm(IServiceCollection services) {
        services.AddSingleton<Counterparty>(_ => new Counterparty());
        
        services.AddSingleton<CounterpartyEnumsVm>();
        services.AddSingleton<SaveCpFormButVm>();
        services.AddSingleton<SaveCpFormAction>();

        
        services.AddSingleton<FormCp>();
        
        services.AddSingleton<EditCpFormState>();
        services.AddSingleton<CreateCpFormState>();        
        
        services.AddSingleton<BaseCpFormState, EditCpFormState>();
        services.AddSingleton<BaseCpFormState, CreateCpFormState>();

        
        services.AddTransient<CounterpartyFormWindow>();
    }

    private static void RegisterTable(IServiceCollection services) {
        services.AddSingleton<CounterpartyDataGridVm>();
        
        services.AddSingleton<AddCounterpartyButVm>();
        services.AddSingleton<EditCounterpartyButVm>();
        services.AddSingleton<DeleteCounterpartyButVm>();

        services.AddSingleton<AddCounterpartyAction>();
        services.AddSingleton<EditCounterpartyAction>();

        
        services.AddSingleton<TableCounterparty>();
        
        services.AddSingleton<EditCpTableState>();
        services.AddSingleton<BaseCpTableState, EditCpTableState>();
        
        
        services.AddTransient<CounterpartyTableWindow>();
    }
}