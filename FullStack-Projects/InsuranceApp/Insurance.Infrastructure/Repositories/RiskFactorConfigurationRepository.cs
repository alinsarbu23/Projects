using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class RiskFactorConfigurationRepository: IRiskFactorConfigurationRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public RiskFactorConfigurationRepository(InsuranceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<RiskFactorConfiguration>> GetActiveForBuildingAsync(
            Guid countryId,
            Guid countyId,
            Guid cityId,
            Guid buildingTypeId)
    {
        var daos = await _context.RiskFactorConfigurations.AsNoTracking()
            .Where(x =>
                x.IsActive &&
                (
                    (x.Level == "Country" && x.ReferenceId == countryId) ||
                    (x.Level == "County" && x.ReferenceId == countyId) ||
                    (x.Level == "City" && x.ReferenceId == cityId) ||
                    (x.Level == "BuildingType" && x.ReferenceId == buildingTypeId)
                ))
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<RiskFactorConfiguration>>(daos);
    }

    public async Task<IReadOnlyList<RiskFactorConfiguration>> GetAllAsync()
    {
        var daos = await _context.RiskFactorConfigurations.AsNoTracking().ToListAsync();
        return _mapper.Map<IReadOnlyList<RiskFactorConfiguration>>(daos);
    }

    public async Task<RiskFactorConfiguration?> GetByIdAsync(Guid id)
    {
        var dao = await _context.RiskFactorConfigurations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return _mapper.Map<RiskFactorConfiguration?>(dao);
    }

    public async Task AddAsync(RiskFactorConfiguration riskFactor)
    {
        var dao = _mapper.Map<DAOs.RiskFactorConfiguration>(riskFactor);
        await _context.RiskFactorConfigurations.AddAsync(dao);
    }

    public async Task UpdateAsync(RiskFactorConfiguration riskFactor)
    {
        var dao = await _context.RiskFactorConfigurations.FirstAsync(x => x.Id == riskFactor.Id);

        _mapper.Map(riskFactor, dao);
    }
}
