namespace Insurance.Domain.Models;

using Insurance.Domain.Enums;

public class Building
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }
    public Guid CityId { get; set; }

    public BuildingType BuildingType { get; set; }

    public string Street { get; set; } = null!;
    public string StreetNumber { get; set; } = null!;

    public int? ConstructionYear { get; set; }
    public int? NumberOfFloors { get; set; }

    public decimal SurfaceArea { get; set; }
    public decimal InsuredValue { get; set; }

    public bool FloodRisk { get; set; }
    public bool EarthquakeRisk { get; set; }
}
