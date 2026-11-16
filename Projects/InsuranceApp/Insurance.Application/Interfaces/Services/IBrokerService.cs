using Insurance.Application.DTOs.Brokers;

namespace Insurance.Application.Interfaces.Services;

public interface IBrokerService
{
    Task<Guid> CreateAsync(CreateBrokerDto dto);

    Task UpdateAsync(Guid brokerId, UpdateBrokerDto dto);

    Task ActivateAsync(Guid brokerId);

    Task DeactivateAsync(Guid brokerId);

    Task<GetBrokerByIdDto> GetByIdAsync(Guid brokerId);

    Task<IReadOnlyList<GetBrokersDto>> GetAllAsync();
}
