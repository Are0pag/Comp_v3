using System.Windows;
using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Comp_v4.TableWindows;
using Comp_v4.TableWindows.ConditionalDesignation;
using Comp_v4.TableWindows.ConditionalDesignation.Overrided;
using Comp_v4.TableWindows.Manufacturers;
using Comp_v4.TableWindows.Manufacturers.Overrided;
using Comp_v4.TableWindows.MeasurementUnits;
using Comp_v4.TableWindows.TypeSizes;
using Comp.Db;
using Comp.Db.Contracts;
using Comp.Db.Repositories;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;
using WPF.Services;
using WPF.Templates.TableWindow.v1.Entities.InputHandlers;
using WPF.Templates.TableWindow.v1.Operations.Actions;
using WPF.Templates.TableWindow.v1.Vm.Components;

namespace Comp_v4.CompCard._Installers;

public class CompCardWindowInstaller : AbstractInstaller
{
    protected readonly AreopagContainer _rootContainer;
    protected readonly Dictionary<Type, AreopagContainer> _subContainers;

    public CompCardWindowInstaller(AreopagContainer rootContainer, Dictionary<Type, AreopagContainer> subContainers) {
        _rootContainer = rootContainer;
        _subContainers = subContainers;
    }

    protected override void InstallBindings(AreopagContainer container) {
        InstallTableWindowScope<CondDesignTableWindow>(new TableWindowInstaller<CondDesignTableWindow, ConditionalDesignation, CdValidator, CdFilter>());
        InstallTableWindowScope<ManufacturersTableWindow>(new TableWindowInstaller<ManufacturersTableWindow, Manufacturer, mValidator, mFilter>());
        InstallTableWindowScope<MeasurementUnitTableWindow>(new TableWindowInstaller<MeasurementUnitTableWindow, MeasurementUnit, muValidator, muFilter>());
        InstallTableWindowScope<TypeSizesTableWindow>(new TableWindowInstaller<TypeSizesTableWindow, TypeSize, tsValidator, tsFilter>());


        container.Add<AppDbContext>().AsSingleton().UsingFactoryMethod(() => _rootContainer.Resolve<AppDbContext>());
        container.Add<IRepository<Component>>().To<DbRepository<Component>>()
                 .AsTransient();
        container.Add<IRepository<Category>>().To<DbRepository<Category>>()
                 .AsTransient();
        
        
        
        container.Add<CdFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new CdFieldVm( OpenTableWindow<CondDesignTableWindow, ConditionalDesignation>));
        
        container.Add<ManFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new ManFieldVm( OpenTableWindow<ManufacturersTableWindow, Manufacturer>));

        container.Add<MuFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new MuFieldVm( OpenTableWindow<MeasurementUnitTableWindow, MeasurementUnit>));

        container.Add<TsFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new TsFieldVm( OpenTableWindow<TypeSizesTableWindow, TypeSize>));


        container.Add<ValidatorName>().AsScoped<CompCardWindow>();
        container.Add<ValidatorNomNumber>().AsScoped<CompCardWindow>();
        
        container.Add<NameFieldVm>().AsScoped<CompCardWindow>();
        container.Add<NomenclatureNumberFieldVm>().AsScoped<CompCardWindow>();
        
        container.Add<ValidatorCardComp>().AsScoped<CompCardWindow>();
        
        container.Add<SaveCompButtonVm>().AsScoped<CompCardWindow>();
        container.Add<SaveComponentAction>().AsScoped<CompCardWindow>();
        
        container.Add<CompCardWindow>().AsTransient();
    }
    
    protected void InstallTableWindowScope<TWindow>(AbstractInstaller tableWindowInstaller) 
        where TWindow : Window, IDisposable
    {
        var cdSubContainer = new AreopagContainer();
        cdSubContainer.Add<AppDbContext>().AsSingleton().UsingFactoryMethod(() => _rootContainer.Resolve<AppDbContext>());
        tableWindowInstaller.Install(cdSubContainer);
        _subContainers[typeof(TWindow)] = cdSubContainer;
    }
    
    protected void OpenTableWindow<TWindow, TData>()
        where TWindow : Window, IDisposable
        where TData : class, IDbEntity, new() 
    {
        var contextContainer = _subContainers[typeof(TWindow)];
        var window = contextContainer.BeginScope<TWindow>();
        window.Closed += (sender, args) => {
            contextContainer.ReleaseScope<TWindow>();
        };
        contextContainer
           .Instantiate<ActionStackTracker,
                PersistenceManager<TWindow, TData>,
                TableCommandBinder<TWindow, TData>,
                ActionFilter<TWindow, TData, FiltersVmBase>
            >();
        window.Show();
    }
}