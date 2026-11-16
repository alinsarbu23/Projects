using Insurance.Application.DTOs.RiskFactors;

namespace Insurance.Application.Interfaces.Services;

public interface IRiskFactorConfigurationService
{
    Task<Guid> CreateAsync(CreateRiskFactorConfigurationDto dto);

    Task UpdateAsync(Guid riskFactorId, UpdateRiskFactorConfigurationDto dto);

    Task<GetRiskFactorConfigurationByIdDto> GetByIdAsync(Guid riskFactorId);

    Task<IReadOnlyList<GetRiskFactorConfigurationsDto>> GetAllAsync();
}
