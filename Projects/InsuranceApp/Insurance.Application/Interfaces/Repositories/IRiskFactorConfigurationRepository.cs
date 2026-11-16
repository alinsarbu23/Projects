using Insurance.Domain.Models;

namespace Insurance.Application.Interfaces.Repositories;

public interface IRiskFactorConfigurationRepository
{
    Task<IReadOnlyList<RiskFactorConfiguration>> GetActiveForBuildingAsync(
        Guid countryId,
        Guid countyId,
        Guid cityId,
        Guid buildingTypeId);

    Task<IReadOnlyList<RiskFactorConfiguration>> GetAllAsync();

    Task<RiskFactorConfiguration?> GetByIdAsync(Guid id);

    Task AddAsync(RiskFactorConfiguration riskFactor);

    Task UpdateAsync(RiskFactorConfiguration riskFactor);
}
