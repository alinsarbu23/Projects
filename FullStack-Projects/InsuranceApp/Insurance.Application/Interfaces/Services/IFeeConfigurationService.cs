using Insurance.Application.DTOs.Fees;

namespace Insurance.Application.Interfaces.Services;

public interface IFeeConfigurationService
{
    Task<Guid> CreateAsync(CreateFeeConfigurationDto dto);

    Task UpdateAsync(Guid feeConfigurationId, UpdateFeeConfigurationDto dto);

    Task<GetFeeConfigurationByIdDto> GetByIdAsync(Guid feeConfigurationId);

    Task<IReadOnlyList<GetFeeConfigurationsDto>> GetAllAsync();
}
