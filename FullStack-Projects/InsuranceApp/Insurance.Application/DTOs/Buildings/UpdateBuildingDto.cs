namespace Insurance.Application.DTOs.Buildings;

public class UpdateBuildingDto
{
    public required string Street { get; set; }

    public required string StreetNumber { get; set; }

    public required int ConstructionYear { get; set; }

    public required int NumberOfFloors { get; set; }

    public required decimal SurfaceArea { get; set; }

    public required decimal InsuredValue { get; set; }

    public required bool FloodRisk { get; set; }

    public required bool EarthquakeRisk { get; set; }
}
