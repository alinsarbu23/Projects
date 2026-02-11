using AutoMapper;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class BuildingService : IBuildingService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BuildingService> _logger;
    public BuildingService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<BuildingService> logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> CreateBuildingAsync(Guid clientId, CreateBuildingDto dto)
    {
        _logger.LogInformation(
            "Creating building for Client {ClientId} (CityId={CityId}, BuildingTypeId={BuildingTypeId})",
            clientId,
            dto.CityId,
            dto.BuildingTypeId);

        var clientExists = await _unitOfWork.Clients.ExistsAsync(clientId);
        if (!clientExists)
        {
            _logger.LogWarning("Create building failed. Client {ClientId} not found", clientId);
            throw new NotFoundException("Client not found.");
        }

        if (!await _unitOfWork.Cities.ExistsAsync(dto.CityId))
        {
            _logger.LogWarning("Create building failed. Invalid CityId {CityId}", dto.CityId);
            throw new ArgumentException("Invalid city.");
        }

        if (!await _unitOfWork.BuildingTypes.ExistsAsync(dto.BuildingTypeId))
        {
            _logger.LogWarning("Create building failed. Invalid BuildingTypeId {BuildingTypeId}", dto.BuildingTypeId);
            throw new ArgumentException("Invalid building type.");
        }

        var building = _mapper.Map<Building>(dto);
        building.Id = Guid.NewGuid();
        building.ClientId = clientId;

        await _unitOfWork.Buildings.AddAsync(building);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while saving building for Client {ClientId}", clientId);
            throw new InvalidOperationException("Failed to create building.");
        }

        _logger.LogInformation("Building {BuildingId} created successfully", building.Id);

        return building.Id;
    }

    public async Task<GetBuildingByIdDto> GetBuildingByIdAsync(Guid buildingId)
    {
        _logger.LogInformation("Retrieving building {BuildingId}", buildingId);

        var building = await _unitOfWork.Buildings.GetByIdAsync(buildingId);

        if (building == null)
        {
            _logger.LogWarning("Building {BuildingId} not found", buildingId);
            throw new NotFoundException($"Building with id {buildingId} not found.");
        }

        _logger.LogInformation("Building {BuildingId} retrieved successfully", buildingId);

        return _mapper.Map<GetBuildingByIdDto>(building);
    }

    public async Task<IReadOnlyList<ClientBuildingSummaryDto>> GetBuildingsForClientAsync(Guid clientId)
    {
        _logger.LogInformation("Retrieving buildings for Client {ClientId}", clientId);

        var buildings = await _unitOfWork.Buildings.GetBuildingsByClientIdAsync(clientId);

        _logger.LogInformation(
            "Retrieved {Count} buildings for Client {ClientId}",
            buildings.Count,
            clientId);

        return _mapper.Map<IReadOnlyList<ClientBuildingSummaryDto>>(buildings);
    }

    public async Task UpdateBuildingAsync(Guid buildingId, UpdateBuildingDto dto)
    {
        _logger.LogInformation("Updating building {BuildingId}", buildingId);

        var building = await _unitOfWork.Buildings.GetByIdAsync(buildingId);

        if (building == null)
        {
            _logger.LogWarning("Update failed. Building {BuildingId} not found", buildingId);
            throw new NotFoundException($"Building with id {buildingId} not found.");
        }

        _mapper.Map(dto, building);

        await _unitOfWork.Buildings.UpdateAsync(building);

        var rowsAffected = await _unitOfWork.SaveChangesAsync();

        if (rowsAffected == 0)
        {
            _logger.LogError("Critical error while updating building {BuildingId}", buildingId);
            throw new InvalidOperationException("Failed to update building.");
        }

        _logger.LogInformation("Building {BuildingId} updated successfully", buildingId);
    }
}
