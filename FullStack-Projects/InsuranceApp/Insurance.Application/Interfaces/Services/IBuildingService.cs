using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;

namespace Insurance.Application.Interfaces.Services;

public interface IBuildingService
{
    Task<Guid> CreateBuildingAsync(Guid clientId, CreateBuildingDto dto);
    Task UpdateBuildingAsync(Guid buildingId, UpdateBuildingDto dto);
    Task<GetBuildingByIdDto> GetBuildingByIdAsync(Guid buildingId);
    Task<IReadOnlyList<ClientBuildingSummaryDto>> GetBuildingsForClientAsync(Guid clientId);
}
