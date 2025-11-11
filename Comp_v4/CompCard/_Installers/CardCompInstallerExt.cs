using Comp_v4.CompCard.Entities;
using Comp_v4.CompCard.Entities.States;
using Comp_v4.CompCard.Entities.Validation;
using Comp_v4.CompCard.Operations.Actions;
using Comp_v4.CompCard.Vm;
using Comp_v4.CompCard.Vm.Buttons;
using Microsoft.Extensions.DependencyInjection;

namespace Comp_v4.CompCard._Installers;

public static class CardCompInstallerExt
{
    public static void RegisterCardComp(this IServiceCollection services) {
        States(services);
        Validation(services);
        TextFields(services);
        Urls(services);
        Images(services);
        FieldsWithButsToNestedWindows(services);

        services.AddSingleton<CardCopmEditController>();

        services.AddSingleton<SaveCompButtonVm>();
        services.AddSingleton<SaveComponentAction>();
        
        services.AddTransient<CompCardWindow>();
    }

    private static void FieldsWithButsToNestedWindows(IServiceCollection services) {
        services.AddSingleton<CdFieldVm>((provider) => {
            return new CdFieldVm(null);
        });
        services.AddSingleton<ManFieldVm>(provider => {
            return new ManFieldVm(() => {
                //ResolveTableWindow<ManufacturersTableWindow, Manufacturer>(provider);
            });
        });
        
        services.AddSingleton<MuFieldVm>(provider => {
            return new MuFieldVm(() => {
                //ResolveTableWindow<MeasurementUnitTableWindow, MeasurementUnit>(provider);
            });
        });
        
        services.AddSingleton<TsFieldVm>(provider => {
            return new TsFieldVm(() => {
                //ResolveTableWindow<TypeSizesTableWindow, TypeSize>(provider);
            });
        });
        
        services.AddSingleton<GpsFieldVm>(provider => {
            return new GpsFieldVm(() => {
                //ResolveTableWindow<GenericParametersSetsWindow, GenericParametersSet>(provider);
            });
        });
    }

    private static void Images(IServiceCollection services) {
        services.AddSingleton<ImageFieldVmBase, ImageFieldVm>();
        services.AddSingleton<SelectImageAction>();
        services.AddSingleton<OpenImageAction>();
        services.AddSingleton<ClearImageAction>();
    }

    private static void Urls(IServiceCollection services) {
        services.AddSingleton<UrlFieldControlVm>();
        services.AddSingleton<UrlAlternativeFieldControlVm>();
        services.AddSingleton<FilePathFieldControlVm>();
        services.AddSingleton<SetUrlAction>();
        services.AddSingleton<SetUrlAlternativeAction>();
        services.AddSingleton<SetFilePathAction>();
    }

    private static void TextFields(IServiceCollection services) {
        services.AddSingleton<NameFieldVm>();
        services.AddSingleton<NomenclatureNumberFieldVm>();
        services.AddSingleton<CatalogNumberFieldVm>();
        services.AddSingleton<LabelingOptionsFieldVm>();
        services.AddSingleton<CodeOfElementFieldVm>();
        services.AddSingleton<QrCodeDataFieldVm>();
        services.AddSingleton<DescriptionFieldVm>();
        services.AddSingleton<CommentsFieldVm>();
        services.AddSingleton<gpMainFieldVm>();
        services.AddSingleton<gp1FieldVm>();
        services.AddSingleton<gp2FieldVm>();
        services.AddSingleton<gp3FieldVm>();
        services.AddSingleton<gp4FieldVm>();
        services.AddSingleton<gp5FieldVm>();
    }

    private static void Validation(IServiceCollection services) {
        services.AddSingleton<ValidatorName>();
        services.AddSingleton<ValidatorNomNumber>();
        services.AddSingleton<ValidatorCatalogNumber>();
        services.AddSingleton<ValidatorLabelingOptions>();
        services.AddSingleton<ValidatorCodeOfElement>();
        services.AddSingleton<ValidatorQrCodeData>();
        services.AddSingleton<ValidatorDescription>();
        services.AddSingleton<ValidatorComments>();
        services.AddSingleton<ValidatorGp>();
        services.AddSingleton<ValidatorUrl>();
    }

    private static void States(IServiceCollection services) {
        services.AddSingleton<EditStateCardComp>();
        services.AddSingleton<CreateStateCardComp>();

        services.AddSingleton<BaseStateCardComp, EditStateCardComp>();
        services.AddSingleton<BaseStateCardComp, CreateStateCardComp>();
        
        services.AddSingleton<CardComp>();
    }
}