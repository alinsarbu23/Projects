namespace Insurance.Application.DTOs.Buildings;

public class CreateBuildingDto
{
    public required Guid CityId { get; init; }
    public required Guid BuildingTypeId { get; init; }
    public required string Street { get; init; }
    public required string StreetNumber { get; init; }
    public int? ConstructionYear { get; init; }
    public int? NumberOfFloors { get; init; }
    public required decimal SurfaceArea { get; init; }
    public required decimal InsuredValue { get; init; }
    public bool FloodRisk { get; init; }
    public bool EarthquakeRisk { get; init; }
}
