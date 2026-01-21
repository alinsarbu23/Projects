using AutoMapper;
using Insurance.Application.DTOs.Buildings;
using Insurance.Application.DTOs.Clients;
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

    public async Task<Guid> CreateBuildingAsync(Guid clientId, CreateBuildingDto createBuildingDto)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
        if(client == null)
        {
            throw new KeyNotFoundException($"Client with id {clientId} not found.");
        }

        var building = new Building
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            CityId = createBuildingDto.CityId,
            BuildingTypeId = createBuildingDto.BuildingTypeId,
            Street = createBuildingDto.Street,
            StreetNumber = createBuildingDto.StreetNumber,
            ConstructionYear = createBuildingDto.ConstructionYear,
            NumberOfFloors = createBuildingDto.NumberOfFloors,
            SurfaceArea = createBuildingDto.SurfaceArea,
            InsuredValue = createBuildingDto.InsuredValue,
            FloodRisk = createBuildingDto.FloodRisk,
            EarthquakeRisk = createBuildingDto.EarthquakeRisk
        };

        await _unitOfWork.Buildings.AddAsync(building);
        var rowsAffected = await _unitOfWork.SaveChangesAsync();

        if(rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to create building.");
        }

        return building.Id;
    }

    public async Task<GetBuildingByIdDto> GetBuildingByIdAsync(Guid buildingId)
    {
        var building = await _unitOfWork.Buildings.GetByIdAsync(buildingId);
        if(building == null)
        {
            throw new KeyNotFoundException($"Building with id {buildingId} not found.");
        }

        return _mapper.Map<GetBuildingByIdDto>(building);
    }

    public async Task<IReadOnlyList<ClientBuildingSummaryDto>> GetBuildingsForClientAsync(Guid clientId)
    {
        var buildings = await _unitOfWork.Buildings.GetBuildingsByClientIdAsync(clientId);
        return _mapper.Map<IReadOnlyList<ClientBuildingSummaryDto>>(buildings);
    }

    public async Task UpdateBuildingAsync(Guid buildingId, UpdateBuildingDto updateBuildingDto)
    {
        var building  = await _unitOfWork.Buildings.GetByIdAsync(buildingId);
        if(building == null)
        {
            throw new KeyNotFoundException($"Building with id {buildingId} not found.");
        }

        building.Street = updateBuildingDto.Street;
        building.StreetNumber = updateBuildingDto.StreetNumber;
        building.ConstructionYear = updateBuildingDto.ConstructionYear;
        building.NumberOfFloors = updateBuildingDto.NumberOfFloors;
        building.SurfaceArea = updateBuildingDto.SurfaceArea;
        building.InsuredValue = updateBuildingDto.InsuredValue;
        building.FloodRisk = updateBuildingDto.FloodRisk;
        building.EarthquakeRisk = updateBuildingDto.EarthquakeRisk;

        await _unitOfWork.Buildings.UpdateAsync(building);
        var rowsAffected = await _unitOfWork.SaveChangesAsync();

        if(rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to update building.");
        }
    }
}
