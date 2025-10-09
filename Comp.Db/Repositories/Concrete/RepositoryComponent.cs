using Comp.Db.Contracts;
using Comp.ModelData.Comp;
using Comp.ModelData.SortingItems;
using Comp.ModelData.TechnicalItems;

namespace Comp.Db.Repositories.Concrete;

public class RepositoryComponent : DbRepository<Component>
{
    protected readonly IRepository<Category> _categoryRepository;
    protected readonly IRepository<ConditionalDesignation> _conditionalDesignationRepository;
    protected readonly IRepository<Manufacturer> _manufacturerRepository;
    protected readonly IRepository<MeasurementUnit> _measurementUnitRepository;
    protected readonly IRepository<TypeSize> _typeSizeRepository;
    
    public RepositoryComponent(AppDbContext context, 
                               IRepository<Category> categoryRepository, 
                               IRepository<ConditionalDesignation> conditionalDesignationRepository, 
                               IRepository<Manufacturer> manufacturerRepository, 
                               IRepository<MeasurementUnit> measurementUnitRepository, 
                               IRepository<TypeSize> typeSizeRepository) : base(context) {
        _categoryRepository = categoryRepository;
        _conditionalDesignationRepository = conditionalDesignationRepository;
        _manufacturerRepository = manufacturerRepository;
        _measurementUnitRepository = measurementUnitRepository;
        _typeSizeRepository = typeSizeRepository;
    }

    public override async Task<List<Component>> GetAllAsync() {
        var copms = await base.GetAllAsync();
        foreach (var copm in copms) {
            copm.Category = await _categoryRepository.GetByIdAsync(copm.CategoryId);

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
            
            dbInstance.ConditionalDesignationId = entity.ConditionalDesignation?.Id ?? null;
            dbInstance.ManufacturerId = entity.Manufacturer?.Id ?? null;
            dbInstance.MeasurementUnitId = entity.MeasurementUnit?.Id ?? null;
            dbInstance.TypeSizeId = entity.TypeSize?.Id ?? null;
            
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
            GpMain = c.GpMain,
            Gp1 = c.Gp1,
            Gp2 = c.Gp2,
            Gp3 = c.Gp3,
            Gp4 = c.Gp4,
            Gp5 = c.Gp5,
        };
    }
}