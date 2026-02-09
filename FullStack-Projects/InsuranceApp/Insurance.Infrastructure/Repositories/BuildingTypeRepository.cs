using Insurance.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class BuildingTypeRepository : IBuildingTypeRepository
{
    private readonly InsuranceDbContext _context;

    public BuildingTypeRepository(InsuranceDbContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(Guid id)
    {
        return _context.BuildingTypes.AsNoTracking().AnyAsync(buildingType => buildingType.Id == id);
    }
}
