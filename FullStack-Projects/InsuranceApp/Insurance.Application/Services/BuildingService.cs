using AutoMapper;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Models;

namespace Insurance.Application.Services;

public class BuildingService : IBuildingService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public BuildingService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateBuildingAsync(Guid clientId, CreateBuildingDto dto)
    {
        var clientExists = await _unitOfWork.Clients.ExistsAsync(clientId);
        if (!clientExists)
        {
            throw new NotFoundException("Client not found.");
        }

        if (!await _unitOfWork.Cities.ExistsAsync(dto.CityId))
            throw new ArgumentException("Invalid city.");

        if (!await _unitOfWork.BuildingTypes.ExistsAsync(dto.BuildingTypeId))
            throw new ArgumentException("Invalid building type.");

        var building = _mapper.Map<Building>(dto);
        building.Id = Guid.NewGuid();
        building.ClientId = clientId;

        await _unitOfWork.Buildings.AddAsync(building);
        await _unitOfWork.SaveChangesAsync();

        return building.Id;
    }

    public async Task<GetBuildingByIdDto> GetBuildingByIdAsync(Guid buildingId)
    {
        var building = await _unitOfWork.Buildings.GetByIdAsync(buildingId);
        if (building == null)
        {
            throw new NotFoundException($"Building with id {buildingId} not found.");
        }

        return _mapper.Map<GetBuildingByIdDto>(building);
    }

    public async Task<IReadOnlyList<ClientBuildingSummaryDto>> GetBuildingsForClientAsync(Guid clientId)
    {
        var buildings = await _unitOfWork.Buildings.GetBuildingsByClientIdAsync(clientId);
        return _mapper.Map<IReadOnlyList<ClientBuildingSummaryDto>>(buildings);
    }

    public async Task UpdateBuildingAsync(Guid buildingId, UpdateBuildingDto dto)
    {
        var building = await _unitOfWork.Buildings.GetByIdAsync(buildingId);
        if (building == null)
        {
            throw new NotFoundException($"Building with id {buildingId} not found.");
        }

        _mapper.Map(dto, building);

        await _unitOfWork.Buildings.UpdateAsync(building);
        var rowsAffected = await _unitOfWork.SaveChangesAsync();

        if (rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to update building.");
        }
    }
}
