using System;
using System.Collections.Generic;

namespace Insurance.Infrastructure.DAOs;

public partial class Building
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public Guid CityId { get; set; }

    public Guid BuildingTypeId { get; set; }

    public string Street { get; set; } = null!;

    public string StreetNumber { get; set; } = null!;

    public int? ConstructionYear { get; set; }

    public int? NumberOfFloors { get; set; }

    public decimal SurfaceArea { get; set; }

    public decimal InsuredValue { get; set; }

    public bool FloodRisk { get; set; }

    public bool EarthquakeRisk { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual BuildingType BuildingType { get; set; } = null!;

    public virtual City City { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}
