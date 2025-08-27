using Comp.ModelData.TechnicalItems;
using CL_CompDb.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CL_CompDb;

public class ConditionalDesignationRepository : DbRepository<ConditionalDesignation>, IConditionalDesignationRepository
{
    public ConditionalDesignationRepository(AppDbContext context) : base(context) { }

    public async Task<List<ConditionalDesignation>> GetByNameAsync(string name) {
        return await _context.ConditionalDesignations
                             .Where(cd => cd.Name.Contains(name))
                             .ToListAsync();
    }

    public async Task<List<ConditionalDesignation>> GetByDesignationAsync(string designation) {
        return await _context.ConditionalDesignations
                             .Where(cd => cd.Designation.Contains(designation))
                             .ToListAsync();
    }
}