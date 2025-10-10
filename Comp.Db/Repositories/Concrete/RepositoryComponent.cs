using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;

namespace Comp.Db.Repositories.Concrete;

public class RepositoryComponent : DbRepository<Component>
{
    protected readonly IRepository<Category> _categoryRepository;
    protected readonly IRepository<GenericParametersSet> _genericParametersSetRepository;
    protected readonly IRepository<ConditionalDesignation> _conditionalDesignationRepository;
    protected readonly IRepository<Manufacturer> _manufacturerRepository;
    protected readonly IRepository<MeasurementUnit> _measurementUnitRepository;
    protected readonly IRepository<TypeSize> _typeSizeRepository;
    
    public RepositoryComponent(AppDbContext context, 
                               IRepository<Category> categoryRepository, 
                               IRepository<ConditionalDesignation> conditionalDesignationRepository, 
                               IRepository<Manufacturer> manufacturerRepository, 
                               IRepository<MeasurementUnit> measurementUnitRepository, 
                               IRepository<TypeSize> typeSizeRepository, 
                               IRepository<GenericParametersSet> genericParametersSetRepository) : base(context) {
        _categoryRepository = categoryRepository;
        _conditionalDesignationRepository = conditionalDesignationRepository;
        _manufacturerRepository = manufacturerRepository;
        _measurementUnitRepository = measurementUnitRepository;
        _typeSizeRepository = typeSizeRepository;
        _genericParametersSetRepository = genericParametersSetRepository;
    }

    public override async Task<List<Component>> GetAllAsync() {
        var copms = await base.GetAllAsync();
        foreach (var copm in copms) {
            if (await _categoryRepository.GetByIdAsync(copm.CategoryId) is not {} category)
                throw new InvalidOperationException("Category can not be null");
            
            copm.Category = category;

            if (copm.GenericParametersSetId != null)
                copm.GenericParametersSet = await _genericParametersSetRepository.GetByIdAsync(copm.GenericParametersSetId.Value);

            if (copm.ConditionalDesignationId != null) {
                int id = copm.ConditionalDesignationId.Value;
                copm.ConditionalDesignation = await _conditionalDesignationRepository.GetByIdAsync(id);
            }

            if (copm.ManufacturerId != null) {
                int id = copm.ManufacturerId.Value;
                copm.Manufacturer = await _manufacturerRepository.GetByIdAsync(id);
            }

            if (copm.MeasurementUnitId != null) {
                int id = copm.MeasurementUnitId.Value;
                copm.MeasurementUnit = await _measurementUnitRepository.GetByIdAsync(id);
            }

            if (copm.TypeSizeId != null) {
                int id = copm.TypeSizeId.Value;
                copm.TypeSize = await _typeSizeRepository.GetByIdAsync(id);
            }
            
        }
        return copms;
    }

    public override async Task AddAsync(Component entity) {
        var dbEntity = GetDbCloneOnAdding(entity);
        await base.AddAsync(dbEntity);
    }

    public override async Task UpdateAsync(Component entity) {
        try {
            if (await GetByIdAsync(entity.Id) is not {} dbInstance)
                throw new NullReferenceException("Db Instance could not be found");
            
            dbInstance.CategoryId = entity.CategoryId;
            dbInstance.GenericParametersSetId = entity.GenericParametersSet?.Id ?? null;
            dbInstance.ConditionalDesignationId = entity.ConditionalDesignation?.Id ?? null;
            dbInstance.ManufacturerId = entity.Manufacturer?.Id ?? null;
            dbInstance.MeasurementUnitId = entity.MeasurementUnit?.Id ?? null;
            dbInstance.TypeSizeId = entity.TypeSize?.Id ?? null;
            
            if (dbInstance.CategoryId == 0)
                throw new InvalidOperationException("Category can not be null");
            
            dbInstance.Name = entity.Name;
            dbInstance.NomenclatureNumber = entity.NomenclatureNumber;
            
            dbInstance.CatalogNumber = entity.CatalogNumber;
            dbInstance.LabelingOptions = entity.LabelingOptions;
            dbInstance.CodeOfElement = entity.CodeOfElement;
            
            dbInstance.Url = entity.Url;
            dbInstance.UrlAlternative = entity.UrlAlternative;
            dbInstance.FilePath = entity.FilePath;
            dbInstance.ImagePath = entity.ImagePath;
            
            dbInstance.QrCodeData = entity.QrCodeData;
            dbInstance.Description = entity.Description;
            dbInstance.Comments = entity.Comments;
            
            dbInstance.GpMain = entity.GpMain;
            dbInstance.Gp1 = entity.Gp1;
            dbInstance.Gp2 = entity.Gp2;
            dbInstance.Gp3 = entity.Gp3;
            dbInstance.Gp4 = entity.Gp4;
            dbInstance.Gp5 = entity.Gp5;
            
            _context.Set<Component>().Update(dbInstance);
            await _context.SaveChangesAsync();
        }
        catch (Exception e) {
            Console.WriteLine(e);
        }
    }
    
    public Component GetDbCloneOnAdding(Component c) {
        return new Component() {
            Id = default,
            
            CategoryId = c.Category.Id,
            
            GenericParametersSetId = c.GenericParametersSet?.Id,
            ConditionalDesignationId = c.ConditionalDesignation?.Id,
            ManufacturerId = c.Manufacturer?.Id,
            MeasurementUnitId = c.MeasurementUnit?.Id,
            TypeSizeId = c.TypeSize?.Id,
            
            Name = c.Name,
            NomenclatureNumber = c.NomenclatureNumber,
            
            CatalogNumber = c.CatalogNumber,
            LabelingOptions = c.LabelingOptions,
            CodeOfElement = c.CodeOfElement,
            
            Url = c.Url,
            UrlAlternative = c.UrlAlternative,
            FilePath = c.FilePath,
            ImagePath = c.ImagePath,
            
            QrCodeData = c.QrCodeData,
            Description = c.Description,
            Comments = c.Comments,
            
            GpMain = c.GpMain,
            Gp1 = c.Gp1,
            Gp2 = c.Gp2,
            Gp3 = c.Gp3,
            Gp4 = c.Gp4,
            Gp5 = c.Gp5,
        };
    }
}