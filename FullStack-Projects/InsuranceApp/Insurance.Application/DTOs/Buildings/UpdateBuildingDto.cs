namespace Insurance.Application.DTOs.Buildings;

public class UpdateBuildingDto
{
    public required string Street { get; set; }
    public required string StreetNumber { get; set; }
    public int? ConstructionYear { get; set; }
    public int? NumberOfFloors { get; set; }
    public required decimal SurfaceArea { get; set; }
    public required decimal InsuredValue { get; set; }
    public bool FloodRisk { get; set; }
    public bool EarthquakeRisk { get; set; }
}
