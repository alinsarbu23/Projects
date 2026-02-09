namespace Insurance.Application.DTOs.Buildings;

public class GetBuildingByIdDto
{
    public Guid Id { get; init; }

    public required string Street { get; init; }

    public required string StreetNumber { get; init; }

    public required string City { get; init; }

    public required string County { get; init; }

    public required string Country { get; init; }

    public required string BuildingType { get; init; }

    public int? ConstructionYear { get; init; }

    public int? NumberOfFloors { get; init; }

    public decimal SurfaceArea { get; init; }

    public decimal InsuredValue { get; init; }

    public bool FloodRisk { get; init; }

    public bool EarthquakeRisk { get; init; }
}
