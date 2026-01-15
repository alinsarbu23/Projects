namespace Insurance.Application.DTOs.Buildings;

public class GetBuildingByIdDto
{
    public Guid Id { get; init; }
    public string Street { get; init; }
    public string StreetNumber { get; init; }
    public string City { get; init; }
    public string County { get; init; }
    public string Country { get; init; }
    public string BuildingType { get; init; }
    public int? ConstructionYear { get; init; }
    public int? NumberOfFloors { get; init; }
    public decimal SurfaceArea { get; init; }
    public decimal InsuredValue { get; init; }

    public bool FloodRisk { get; init; }
    public bool EarthquakeRisk { get; init; }
}
