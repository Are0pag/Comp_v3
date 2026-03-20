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
        services.AddScoped<Counterparty>(_ => new Counterparty());
        
        services.AddScoped<CounterpartyEnumsVm>();
        services.AddScoped<SaveCpFormButVm>();
        services.AddScoped<SaveCpFormAction>();

        services.AddScoped<ConfirmSelectionAction>();
        services.AddScoped<ConfirmSelectiontButVm>();

        
        services.AddScoped<FormCp>();
        
        services.AddScoped<EditCpFormState>();
        services.AddScoped<CreateCpFormState>();        
        
        services.AddScoped<BaseCpFormState, EditCpFormState>();
        services.AddScoped<BaseCpFormState, CreateCpFormState>();

        
        services.AddScoped<CounterpartyFormWindow>();
    }

    private static void RegisterTable(IServiceCollection services) {
        services.AddScoped<CounterpartyDataGridVm>();
        
        services.AddScoped<AddCounterpartyButVm>();
        services.AddScoped<EditCounterpartyButVm>();
        services.AddScoped<DeleteCounterpartyButVm>();

        services.AddScoped<AddCounterpartyAction>();
        services.AddScoped<EditCounterpartyAction>();
        services.AddScoped<DeleteCounterpartyAction>();

        
        services.AddScoped<TableCounterparty>();
        
        services.AddScoped<EditCpTableState>();
        services.AddScoped<BaseCpTableState, EditCpTableState>();
        
        
        services.AddScoped<CounterpartyTableWindow>();
    }
}