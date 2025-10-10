using System.Windows;
using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Events;
using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Comp_v4.Installers;
using Comp_v4.TableWindows;
using Comp_v4.TableWindows.ConditionalDesignation;
using Comp_v4.TableWindows.ConditionalDesignation.Overrided;
using Comp_v4.TableWindows.GenericParametersSets;
using Comp_v4.TableWindows.Manufacturers;
using Comp_v4.TableWindows.Manufacturers.Overrided;
using Comp_v4.TableWindows.MeasurementUnits;
using Comp_v4.TableWindows.TypeSizes;
using Comp.Db;
using Comp.ModelData.Comp;
using Comp.ModelData.TechnicalItems;
using Utils.EventBus;
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
        InstallTableWindowScope<GenericParametersSetsWindow>(new TableWindowInstaller<GenericParametersSetsWindow, GenericParametersSet, gpsValidator, gpsFilter>());


        container.Add<AppDbContext>().AsSingleton().UsingFactoryMethod(() => _rootContainer.Resolve<AppDbContext>());
        new CompRepoInstaller().Install(container);


        container.Add<CdFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new CdFieldVm( OpenTableWindow<CondDesignTableWindow, ConditionalDesignation>));
        
        container.Add<ManFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new ManFieldVm( OpenTableWindow<ManufacturersTableWindow, Manufacturer>));

        container.Add<MuFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new MuFieldVm( OpenTableWindow<MeasurementUnitTableWindow, MeasurementUnit>));

        container.Add<TsFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new TsFieldVm( OpenTableWindow<TypeSizesTableWindow, TypeSize>));
        
        container.Add<GpsFieldVm>().AsScoped<CompCardWindow>()
                 .UsingFactoryMethod(() => new GpsFieldVm( OpenTableWindow<GenericParametersSetsWindow, GenericParametersSet>));


        container.Add<Component>()
                  .AsScoped<CompCardWindow>();
        
        container.Add<EditStateCardComp>().AsScoped<CompCardWindow>();
        container.Add<CreateStateCardComp>().AsScoped<CompCardWindow>();
        container.Add<CardComp>().AsScoped<CompCardWindow>();
        
        container.Add<ValidatorName>().AsScoped<CompCardWindow>();
        container.Add<ValidatorNomNumber>().AsScoped<CompCardWindow>();
        container.Add<ValidatorCatalogNumber>().AsScoped<CompCardWindow>();
        container.Add<ValidatorLabelingOptions>().AsScoped<CompCardWindow>();
        container.Add<ValidatorCodeOfElement>().AsScoped<CompCardWindow>();
        container.Add<ValidatorQrCodeData>().AsScoped<CompCardWindow>();
        container.Add<ValidatorDescription>().AsScoped<CompCardWindow>();
        container.Add<ValidatorComments>().AsScoped<CompCardWindow>();
        container.Add<ValidatorGp>().AsTransient();
        
        container.Add<NameFieldVm>().AsScoped<CompCardWindow>();
        container.Add<NomenclatureNumberFieldVm>().AsScoped<CompCardWindow>();
        container.Add<CatalogNumberFieldVm>().AsScoped<CompCardWindow>();
        container.Add<LabelingOptionsFieldVm>().AsScoped<CompCardWindow>();
        container.Add<CodeOfElementFieldVm>().AsScoped<CompCardWindow>();
        container.Add<QrCodeDataFieldVm>().AsScoped<CompCardWindow>();
        container.Add<DescriptionFieldVm>().AsScoped<CompCardWindow>();
        container.Add<CommentsFieldVm>().AsScoped<CompCardWindow>();
        container.Add<gpMainFieldVm>().AsScoped<CompCardWindow>();
        container.Add<gp1FieldVm>().AsScoped<CompCardWindow>();
        container.Add<gp2FieldVm>().AsScoped<CompCardWindow>();
        container.Add<gp3FieldVm>().AsScoped<CompCardWindow>();
        container.Add<gp4FieldVm>().AsScoped<CompCardWindow>();
        container.Add<gp5FieldVm>().AsScoped<CompCardWindow>();
        
        container.Add<CardCopmEditController>().AsScoped<CompCardWindow>();
        
        container.Add<ValidatorUrl>().AsTransient();
        container.Add<UrlFieldControlVm>().AsScoped<CompCardWindow>();
        container.Add<UrlAlternativeFieldControlVm>().AsScoped<CompCardWindow>();
        container.Add<FilePathFieldControlVm>().AsScoped<CompCardWindow>();
        container.Add<SetUrlAction>().AsScoped<CompCardWindow>();
        container.Add<SetUrlAlternativeAction>().AsScoped<CompCardWindow>();
        container.Add<SetFilePathAction>().AsScoped<CompCardWindow>();
        
        container.Add<ImageFieldVm>().AsScoped<CompCardWindow>();
        container.Add<SelectImageAction>().AsScoped<CompCardWindow>();
        container.Add<OpenImageAction>().AsScoped<CompCardWindow>();
        
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
        new TableWindowClosingHandler(window);
        contextContainer
           .Instantiate<ActionStackTracker,
                PersistenceManager<TWindow, TData>,
                TableCommandBinder<TWindow, TData>,
                ActionFilter<TWindow, TData, FiltersVmBase>
            >();
        window.Show();
    }
    
    public class TableWindowClosingHandler : ITableWindowHandler
    {
        protected readonly Window _window;

        public TableWindowClosingHandler(Window window) {
            _window = window;
            EventBus<ICompCardSubscriber>.Subscribe(this);
        }

        public void Dispose() {
            EventBus<ICompCardSubscriber>.Unsubscribe(this);
        }

        public void HandleClosingTableWindow<T>(object? args) where T : Window {
            Application.Current.Dispatcher.BeginInvoke(() => {
                _window.Close();
            });
        }
    }
}

