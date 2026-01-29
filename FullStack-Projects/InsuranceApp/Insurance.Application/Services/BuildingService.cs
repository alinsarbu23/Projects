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

    public async Task<Guid> CreateBuildingAsync(Guid clientId, CreateBuildingDto dto)
    {
        var clientExists = await _unitOfWork.Clients.ExistsAsync(clientId);
        if (!clientExists)
        {
            throw new KeyNotFoundException("Client not found.");
        }

        if (!await _unitOfWork.Cities.ExistsAsync(dto.CityId))
        {
            throw new ArgumentException("Invalid city.");
        }

        if (!await _unitOfWork.BuildingTypes.ExistsAsync(dto.BuildingTypeId))
        {
            throw new ArgumentException("Invalid building type.");
        }

        var building = new Building
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            CityId = dto.CityId,
            BuildingTypeId = dto.BuildingTypeId,
            Street = dto.Street,
            StreetNumber = dto.StreetNumber,
            ConstructionYear = dto.ConstructionYear,
            NumberOfFloors = dto.NumberOfFloors,
            SurfaceArea = dto.SurfaceArea,
            InsuredValue = dto.InsuredValue,
            FloodRisk = dto.FloodRisk,
            EarthquakeRisk = dto.EarthquakeRisk
        };

        await _unitOfWork.Buildings.AddAsync(building);
        await _unitOfWork.SaveChangesAsync();

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

    public async Task UpdateBuildingAsync(Guid buildingId, UpdateBuildingDto dto)
    {
        var building  = await _unitOfWork.Buildings.GetByIdAsync(buildingId);
        if(building == null)
        {
            throw new KeyNotFoundException($"Building with id {buildingId} not found.");
        }

        building.Street = dto.Street;
        building.StreetNumber = dto.StreetNumber;
        building.ConstructionYear = dto.ConstructionYear;
        building.NumberOfFloors = dto.NumberOfFloors;
        building.SurfaceArea = dto.SurfaceArea;
        building.InsuredValue = dto.InsuredValue;
        building.FloodRisk = dto.FloodRisk;
        building.EarthquakeRisk = dto.EarthquakeRisk;

        await _unitOfWork.Buildings.UpdateAsync(building);
        var rowsAffected = await _unitOfWork.SaveChangesAsync();

        if(rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to update building.");
        }
    }
}
