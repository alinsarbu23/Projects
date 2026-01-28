using AutoMapper;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class BuildingRepository : IBuildingRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public BuildingRepository(InsuranceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddAsync(Building entity)
    {
        var buildingDao = _mapper.Map<DAOs.Building>(entity);
        await _context.Buildings.AddAsync(buildingDao);
    }

    public async Task DeleteAsync(Guid id)
    {
        var buildingDao = await _context.Buildings.FirstOrDefaultAsync(building => building.Id == id);
        if(buildingDao != null)
        {
            _context.Buildings.Remove(buildingDao);
        }
    }

    public async Task<IReadOnlyList<Building>> GetAllAsync()
    {
        var buildingsDao = await _context.Buildings.AsNoTracking().ToListAsync();
        return _mapper.Map<IReadOnlyList<Building>>(buildingsDao);
    }

    public async Task<IReadOnlyList<Building>> GetBuildingsByClientIdAsync(Guid clientId)
    {
        var buildingsDao = await _context.Buildings
            .AsNoTracking()
            .Where(b => b.ClientId == clientId)
            .Include(b => b.City)
                .ThenInclude(c => c.County)
                    .ThenInclude(co => co.Country)
            .Include(b => b.BuildingType)
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<Building>>(buildingsDao);
    }

    public async Task<Building?> GetByIdAsync(Guid id)
    {
        var buildingDao = await _context.Buildings
            .AsNoTracking()
            .Include(b => b.City)
                .ThenInclude(c => c.County)
                    .ThenInclude(co => co.Country)
            .Include(b => b.BuildingType)
            .FirstOrDefaultAsync(b => b.Id == id);

        return _mapper.Map<Building?>(buildingDao);
    }



    public Task UpdateAsync(Building entity)
    {
        var buildingDao = _mapper.Map<DAOs.Building>(entity);
        _context.Buildings.Update(buildingDao);
        return Task.CompletedTask;
    }
}